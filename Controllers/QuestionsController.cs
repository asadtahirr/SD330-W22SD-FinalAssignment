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

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string tag)
        {
            List<QuestionSummaryViewModel> questions = null;

            if (string.IsNullOrEmpty(tag))
            {
                questions = await DbContext.Questions.Select(q => new QuestionSummaryViewModel()
                {
                    Id = q.Id,
                    Title = q.Title,
                    QuestionCreator = q.CreatedBy.UserName,
                    AnswersCount = q.Answers.Count(),
                    Tags = q.Tags.ToList()
                }).ToListAsync();
            }
            else
            {
                questions = await DbContext.Questions.Where(q => q.Tags.Any(t => t.Name == tag)).Select(q => new QuestionSummaryViewModel()
                {
                    Id = q.Id,
                    Title = q.Title,
                    QuestionCreator = q.CreatedBy.UserName,
                    AnswersCount = q.Answers.Count(),
                    Tags = q.Tags.ToList()
                }).ToListAsync();
            }

            QuestionIndexViewModel viewModel = new QuestionIndexViewModel();

            viewModel.SummaryOfQuestions = questions;

            return View(viewModel);
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
            Question question = await DbContext.Questions
                                                    .Include(q => q.Tags)
                                                    .Include(q => q.Answers)
                                                    .ThenInclude(a => a.Comments)
                                                    .Include(q => q.Comments)
                                                    .Include(q => q.CreatedBy)
                                                    .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return Redirect("/questions/new");

            QuestionDetailsViewModel viewModel = new QuestionDetailsViewModel()
            {
                Id = question.Id,
                Title = question.Title,
                Body = question.Body,
                Tags = question.Tags,
                Answers = question.Answers,
                Comments = question.Comments
            };

            return View(viewModel);
        }
    }
}
