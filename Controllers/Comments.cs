using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    [EnableCors("myRules")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {
        public readonly VerceldbContext _dbcontext;

        public CommentsController(VerceldbContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        [Route("ListComments")]
        public async Task<IActionResult> ListComments()
        {
            try
            {
                List<Comment> list = await _dbcontext.Comments.ToListAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{commentId:int}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            Comment comment = await _dbcontext.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = comment });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            try
            {
                _dbcontext.Comments.Add(comment);
                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Comment successfully added" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateComment([FromBody] Comment updatedComment)
        {
            Comment oComment = await _dbcontext.Comments.FirstOrDefaultAsync(t => t.CommentId == updatedComment.CommentId);
            if(oComment == null)
            {
                return BadRequest("invalid input");
            }

            try
            {
                oComment.Content = updatedComment.Content is null ? oComment.Content : updatedComment.Content;
                _dbcontext.Comments.Update(oComment);

                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

     
        }
        

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            Comment comment = await _dbcontext.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            try
            {
                _dbcontext.Comments.Remove(comment);
                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Comment successfully deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
