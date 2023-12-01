using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Cors;
using WebApplication7.Resources;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication7.Controllers
{
    [EnableCors("RulesCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly VerceldbContext _dbcontext;

        public UserController(VerceldbContext _context)
        {
            _dbcontext = _context;
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {

            var existingUser = await _dbcontext.Users.FromSqlRaw("SELECT Email FROM USERS WHERE Email LIKE {0}", user.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            user.Password = Utilities.EncryptKey(user.Password);

            try
            {
                _dbcontext.Users.Add(user);
                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, new { message = "User successfully saved" });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while saving the user" });
            }
        }


        [HttpGet]
        [Route("UserList")]
        public async Task<IActionResult> ListUser()
        {
            List<User> list = new List<User>();

            try
            {

                list = await _dbcontext.Users.FromSqlRaw("SELECT * FROM USERS").ToListAsync(); // Asumiendo que ToListAsync() está disponible
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
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
