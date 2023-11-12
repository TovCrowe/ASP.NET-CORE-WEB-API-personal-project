using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace WebApplication7.Controllers
{
    [EnableCors("RulesCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {


        public readonly VerceldbContext _dbcontext;
        public ProjectsController(VerceldbContext context)
        {
            _dbcontext = context;
        }


        [HttpGet]
        [Route("GetProjects")]

        public async Task<IActionResult> ListProjects()
        {

            try
            {
                List<Project> list = await _dbcontext.Projects.Include(o => o.oUser).ToListAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Ok", response = list });

            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status400BadRequest, ex);

            }

        }
        [HttpGet]
        [Route("Get/{projectId:int}")]

        public async Task<IActionResult> GetProjectById(int projectId)
        {
            Project oProjects = await _dbcontext.Projects.FindAsync(projectId);

            if (oProjects == null) {
                return BadRequest("Ïnvalid project");
            }

            try
            {
                oProjects = await _dbcontext.Projects.Where(o => o.ProjectId == projectId).FirstOrDefaultAsync();
                return StatusCode(StatusCodes.Status200OK, new { Message = "ok", response = oProjects });



            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPost]
        [Route("Add")]

        public async Task<IActionResult> AddProject([FromBody] Project project)
        {
            try
            {
                _dbcontext.Projects.Add(project);
                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPut]
        [Route("Edit")]

        public async Task<IActionResult> EditProject([FromBody] Project project)
        {
            Project oProject = await _dbcontext.Projects.FirstOrDefaultAsync(t => t.ProjectId == project.ProjectId);//recupera la tarea existente de la based de datos por su id

            if(oProject == null) {
                return BadRequest($"invalid inputs");

            }

            try
            {
                oProject.NameProject = project.NameProject is null ? oProject.NameProject : project.NameProject;
                oProject.Description = project.Description is null ? oProject.Description : project.Description;

                _dbcontext.Projects.Update(oProject);
                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });

            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
        [HttpDelete]
        [Route("Delte")]

        public async Task<IActionResult> DeleteProject(Project project)
        {
            Project oProject = await _dbcontext.Projects.FindAsync(project.ProjectId);

            if (oProject == null)
            {
                return BadRequest("El proyecto no existe.");
            }

            try
            {
                _dbcontext.Projects.Remove(oProject);
                await _dbcontext.SaveChangesAsync(); // Guarda los cambios en la base de datos

                return Ok("Proyecto eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }






    }
}
