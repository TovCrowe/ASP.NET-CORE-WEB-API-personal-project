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
                list = _dbcontext.Users.Include(c => c.Tasks).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list});
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);


            }

        }


    }
}
