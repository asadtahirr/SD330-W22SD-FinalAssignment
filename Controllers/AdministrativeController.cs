using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stack_overload.Data;
using stack_overload.Models;
using stack_overload.Models.ViewModels;

namespace stack_overload.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrativeController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }

        public AdministrativeController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Questions()
        {
            List<AdministrationQuestionViewModel> questions = await DbContext
                .Questions
                .Include(q => q.CreatedBy)
                .Include(q => q.Tags)
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => new AdministrationQuestionViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt,
                    CreatedByName = q.CreatedBy.UserName,
                    CreatedById = q.CreatedById,
                    AnswersCount = q.Answers.Count(),
                    Tags = q.Tags
                })
                .ToListAsync();

            AdministrationQuestionsViewModel viewModel = new AdministrationQuestionsViewModel
            {
                Questions = questions
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(string id)
        {
            Question question = await DbContext
                                        .Questions
                                        .Include(q => q.Upvoters)
                                        .Include(q => q.Downvoters)
                                        .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return Redirect("~/administrative/questions");
            }

            question.Upvoters.Clear();
            question.Downvoters.Clear();

            DbContext.Questions.Remove(question);

            await DbContext.SaveChangesAsync();

            return Redirect("~/administrative/questions");
        }
    }
}
