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
        public async Task<IActionResult> Index(
            [FromQuery] string tag, [FromQuery] string page, [FromQuery] string sortByMostAnswered
        ) {
            List<QuestionSummaryViewModel> questions = null;
            bool isValidPage = int.TryParse(page, out int pageToVisit);
            bool sortByDate = sortByMostAnswered?.ToLower() == "false" || string.IsNullOrEmpty(sortByMostAnswered);

            if (!isValidPage)
            {
                pageToVisit = 1;
            }

            int questionsToSkip = PaginationViewModel.QuestionsPerPage * (pageToVisit - 1);

            if (string.IsNullOrEmpty(tag))
            {
                IOrderedQueryable<Question> query = sortByDate ? DbContext.Questions.OrderByDescending(q => q.CreatedAt) : DbContext.Questions.OrderByDescending(q => q.Answers.Count());
                
                questions = await query
                                    .Skip(questionsToSkip)
                                    .Take(PaginationViewModel.QuestionsPerPage)
                                    .Select(q => new QuestionSummaryViewModel()
                                    {
                                        Id = q.Id,
                                        Title = q.Title,
                                        QuestionCreator = q.CreatedBy.UserName,
                                        AnswersCount = q.Answers.Count(),
                                        Tags = q.Tags.ToList()
                                    })
                                    .ToListAsync();
            }
            else
            {
                IOrderedQueryable<Question> query = sortByDate ? DbContext.Questions.Where(q => q.Tags.Any(t => t.Name == tag)).OrderByDescending(q => q.CreatedAt) : DbContext.Questions.Where(q => q.Tags.Any(t => t.Name == tag)).OrderByDescending(q => q.Answers.Count());

                questions = await query
                                .Skip(questionsToSkip)
                                .Take(PaginationViewModel.QuestionsPerPage)
                                .Select(q => new QuestionSummaryViewModel()
                                {
                                    Id = q.Id,
                                    Title = q.Title,
                                    QuestionCreator = q.CreatedBy.UserName,
                                    AnswersCount = q.Answers.Count(),
                                    Tags = q.Tags.ToList()
                                })
                                .ToListAsync();
            }

            QuestionIndexViewModel viewModel = new QuestionIndexViewModel();

            viewModel.SummaryOfQuestions = questions;
            viewModel.CurrentPage = pageToVisit;
            viewModel.SortedByDate = sortByDate;
            viewModel.TagForSorting = string.IsNullOrEmpty(tag) ? null : tag;

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
                Comments = question.Comments,
                Votes = question.Votes
            };

            return View(viewModel);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Vote(
            [FromRoute] string id, [FromForm] VoteInputModel inputModel
        ) {
            Question question = await DbContext
                                        .Questions
                                        .Include(q => q.Upvoters)
                                        .Include(q => q.Downvoters)
                                        .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return Redirect("~/questions/index");
            }

            User currentUser = await UserManager.GetUserAsync(User);

            if (question.CreatedById == currentUser.Id)
            {
                return Redirect($"~/questions/details/{id}");
            }
            else
            {
                if (inputModel.Action == "upvote" && (question.Downvoters.Any(v => v.Id == currentUser.Id) || !question.Upvoters.Any(v => v.Id == currentUser.Id)))
                {
                    question.Votes += 1;
                    question.Upvoters.Add(currentUser);
                    question.Downvoters.Remove(currentUser);
                }
                else if (inputModel.Action == "downvote" && (question.Upvoters.Any(v => v.Id == currentUser.Id) || !question.Downvoters.Any(v => v.Id == currentUser.Id)))
                {
                    question.Votes -= 1;
                    question.Downvoters.Add(currentUser);
                    question.Upvoters.Remove(currentUser);
                }
                else
                {
                    return Redirect($"~/questions/details/{id}");
                }

                question.UpdatedAt = DateTime.Now;

                await DbContext.SaveChangesAsync();

                return Redirect($"~/questions/details/{id}");
            }
        }
    }
}
