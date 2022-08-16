using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stack_overload.Data;
using stack_overload.Models;
using stack_overload.Models.InputModels;
using stack_overload.Models.ViewModels;

namespace stack_overload.Controllers
{
    public class AnswersController : Controller
    {
        private UserManager<User> UserManager { get; set; }
        private ApplicationDbContext DbContext { get; set; }

        public AnswersController(UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            UserManager = userManager;
            DbContext = dbContext;
        }

        [HttpGet, Authorize]
        public IActionResult New(string id)
        {
            NewAnswerViewModel viewModel = new NewAnswerViewModel()
            {
                Id = id
            };

            return View(viewModel);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> New(string id, [FromForm] NewAnswerInputModel inputModel)
        {
            User currentUser = await UserManager.GetUserAsync(User);

            Answer answer = new Answer(inputModel.Body, currentUser.Id, id);

            DbContext.Answers.Add(answer);

            await DbContext.SaveChangesAsync();

            return Redirect($"/questions/details/{id}");
        }
    }
}
