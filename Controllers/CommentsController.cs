using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stack_overload.Data;
using stack_overload.Models;
using stack_overload.Models.InputModels;
using stack_overload.Models.ViewModels;

namespace stack_overload.Controllers
{
    public class CommentsController : Controller
    {
        private UserManager<User> UserManager { get; set; }

        private ApplicationDbContext DbContext { get; set; }

        public CommentsController(UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            UserManager = userManager;
            DbContext = dbContext;
        }

        [HttpGet, Authorize]
        public IActionResult New([FromQuery] string questionId, [FromQuery] string answerId)
        {
            NewCommentViewModel viewModel = new NewCommentViewModel();

            if (string.IsNullOrEmpty(answerId) && !string.IsNullOrEmpty(questionId))
            {
                viewModel.QuestionId = questionId;
            }
            else if (!string.IsNullOrEmpty(answerId) && string.IsNullOrEmpty(questionId))
            {
                viewModel.AnswerId = answerId;
            }

            return View(viewModel);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> New([FromQuery] string questionId, [FromQuery] string answerId, [FromForm] NewCommentInputModel inputModel)
        {
            User currentUser = await UserManager.GetUserAsync(User);

            Comment comment = null;

            string url = null;

            if (!string.IsNullOrEmpty(questionId) && string.IsNullOrEmpty(answerId))
            {
                comment = new Comment(inputModel.Body, currentUser.Id, questionId, null);

                url = $"~/questions/details/{questionId}";
            }
            else if (!string.IsNullOrEmpty(answerId) && string.IsNullOrEmpty(questionId))
            {
                comment = new Comment(inputModel.Body, currentUser.Id, null, answerId);

                Answer answer = await DbContext.Answers.FindAsync(answerId);

                url = $"~/questions/details/{answer.QuestionId}";
            }
            else
            {
                return Redirect("questions/index");
            }

            DbContext.Comments.Add(comment);

            await DbContext.SaveChangesAsync();

            return Redirect(url);
        }
    }
}
