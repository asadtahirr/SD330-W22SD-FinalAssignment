using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stack_overload.Data;
using stack_overload.Models;
using stack_overload.Models.InputModels;
using stack_overload.Models.ViewModels;

namespace stack_overload.Controllers
{
    public class QuestionsController : Controller
    {
        private UserManager<User> UserManager { get; set; }

        private ApplicationDbContext DbContext { get; set; }

        public QuestionsController(UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            UserManager = userManager;
            DbContext = dbContext;
        }

        [HttpGet, Authorize]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> New(NewQuestionInputModel inputModel)
        {
            User currentUser = await UserManager.GetUserAsync(User);

            Question question = new Question(inputModel.Title, inputModel.Body, currentUser.Id);

            DbContext.Questions.Add(question);

            foreach (string tagValue in inputModel.Tags.Split(','))
            {
                Tag tag = new Tag(tagValue);

                question.Tags.Add(tag);

                DbContext.Tags.Add(tag);
            }

            DbContext.SaveChanges();

            return Redirect($"/questions/details/{question.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Question question = await DbContext.Questions.Include(q => q.Tags).FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return Redirect("/questions/new");

            QuestionDetailsViewModel viewModel = new QuestionDetailsViewModel()
            {
                Id = question.Id,
                Title = question.Title,
                Body = question.Body,
                Tags = question.Tags
            };

            return View(viewModel);
        }
    }
}
