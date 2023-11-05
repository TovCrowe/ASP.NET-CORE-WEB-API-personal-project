using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly VerceldbContext _dbcontext;

        public UserController(VerceldbContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("UserList")]

        public IActionResult List()
        {
            List<User> list = new List<User>();


            try
            {
                list = _dbcontext.Users.Include(c => c.oTasks).ToList();//enlista todos los usuarios con todas las task que tenga
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);


            }

        }

        [HttpGet]
        [Route("Get/{idUser:int}")]
        public IActionResult Get(int idUser)
        {

            User oUser = _dbcontext.Users.Find(idUser);
            //encontrar al users que le ofrecimos aqui


            if (oUser == null)
            {
                return BadRequest("User not found");//envia un error si no encuentra al usuario
            }
            try
            {
                oUser = _dbcontext.Users.Include(c => c.oTasks).Where(p => p.UserId == idUser).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oUser });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);


            }


        }
    }
}
