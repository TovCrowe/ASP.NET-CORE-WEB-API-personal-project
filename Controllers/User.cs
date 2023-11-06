using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Cors;

namespace WebApplication7.Controllers
{
    [EnableCors("myRules")]
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
        public async Task<IActionResult> ListUser()
        {
            List<User> list = new List<User>();

            try
            {
                list = await _dbcontext.Users.Include(c => c.oTasks).ToListAsync(); // Asumiendo que ToListAsync() está disponible
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);
            }
        }


        [HttpGet]
        [Route("Get/{idUser:int}")]
        public IActionResult GetUser(int idUser)
        {

            User oUser = _dbcontext.Users.Find();
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

        [HttpPost]
        [Route("Add")]
        public IActionResult AddUser([FromBody] User user)
        {

            //encontrar al users que le ofrecimos aqui


            try
            {
                _dbcontext.Users.Add(user);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "User succefully saved" });



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);


            }


        }

        [HttpPut]
        [Route("Edit")]
        public IActionResult EditUser([FromBody] User user)
        {

            User oUser = _dbcontext.Users.Find(user.UserId);

            if (oUser == null)
            {
                return BadRequest("User not found");
            }


            try
            {
                oUser.Name = user.Name is null ? oUser.Name : user.Name;
                oUser.Email = user.Email is null ? oUser.Email : user.Email;
                oUser.Password = user.Password is null ? oUser.Password : user.Password;
                _dbcontext.Users.Update(oUser);
                _dbcontext.SaveChanges();


                return StatusCode(StatusCodes.Status200OK, new { message = "User succefully saved" });



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);


            }


        }

        [HttpDelete]
        [Route("Delete")]

        public IActionResult DeleteUser(User user)
        {
            User oUser = _dbcontext.Users.Find(user.UserId);

            if (oUser == null)
            {
                return BadRequest("User not found");
            }


            try
            {
                _dbcontext.Users.Remove(oUser);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "User succefully saved" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex);


            }
        }




    }
}
