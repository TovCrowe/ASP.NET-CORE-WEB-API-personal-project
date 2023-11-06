using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication7.Controllers2
{
    [EnableCors("myRules")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        public readonly VerceldbContext _dbcontext;//conseguir el context de nuestra database

        public TaskController(VerceldbContext context)//constructor de la clase controller task
        {
            _dbcontext = context;
        }


        [HttpGet]
        [Route("TaskList")]

        public async Task<IActionResult> List()
        {
            try
            {
                List<Models.Task> list = await _dbcontext.Tasks.Include(o => o.oComments).ToListAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }
        [HttpGet]
        [Route("Get/{taskid:int}")]

        public async Task<IActionResult> Get(int taskId)
        {
            Models.Task otask = await _dbcontext.Tasks.FindAsync(taskId);

            if (otask == null)
            {
                return BadRequest("Task not found");
            }

            try
            {
                otask = await _dbcontext.Tasks.Where(p => p.TaskId == taskId).FirstOrDefaultAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = otask });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("Add")]

        public async Task<IActionResult> Add([FromBody] Models.Task task)
        {

            try
            {
                _dbcontext.Add(task);

                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Task succefully saved" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }

        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] Models.Task task)
        {
            Models.Task oTask = await _dbcontext.Tasks.FirstOrDefaultAsync(t => t.TaskId == task.TaskId);//recupera la tarea existente de la based de datos por su id

            if (oTask == null)
            {
                return BadRequest("Ïnvalid users");
            }

            try
            {
                oTask.DueDate = oTask.DueDate is null ? task.DueDate : oTask.DueDate;
                oTask.Status = oTask.Status is null ? task.Status : oTask.Status;
                oTask.Description = oTask.Description is null ? task.Description : oTask.Description;
                oTask.CreatedAt = oTask.CreatedAt is null ? task.CreatedAt : oTask.CreatedAt;
                oTask.Priority = oTask.Priority is null ? task.Priority : oTask.Priority;
                oTask.Title = oTask.Title is null ? task.Title : oTask.Priority;

                _dbcontext.Tasks.Update(oTask);
                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "User succefully saved" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpDelete]
        [Route("Edit")]

        public async Task<IActionResult> Delete(Models.Task task)
        {
            Models.Task oTask = _dbcontext.Tasks.Find(task.TaskId);

            if (oTask == null)
            {
                return BadRequest("User not found");
            }


            try
            {
                _dbcontext.Tasks.Remove(task);
                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "User succefully saved" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);


            }
        }




    }


}

