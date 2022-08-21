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

        [HttpPost, Authorize]
        public async Task<IActionResult> Vote(
            [FromRoute] string id, [FromForm] VoteInputModel inputModel
        )
        {
            Answer answer = await DbContext
                                        .Answers
                                        .Include(q => q.CreatedBy)
                                        .Include(a => a.Upvoters)
                                        .Include(a => a.Downvoters)
                                        .FirstOrDefaultAsync(a => a.Id == id);

            if (answer == null)
            {
                return Redirect("~/questions/index");
            }

            User currentUser = await UserManager.GetUserAsync(User);

            if (answer.CreatedById == currentUser.Id)
            {
                return Redirect($"~/questions/details/{answer.QuestionId}");
            }
            else
            {
                if (inputModel.Action == "upvote")
                {
                    if (answer.Downvoters.Any(v => v.Id == currentUser.Id))
                    {
                        answer.Votes += 1;
                        answer.CreatedBy.Reputation += 5;
                        answer.Downvoters.Remove(currentUser);
                    }
                    else if (!answer.Upvoters.Any(v => v.Id == currentUser.Id))
                    {
                        answer.Votes += 1;
                        answer.CreatedBy.Reputation += 5;
                        answer.Upvoters.Add(currentUser);
                    }
                    else
                    {
                        return Redirect($"~/questions/details/{answer.QuestionId}");
                    }
                }
                else if (inputModel.Action == "downvote")
                {
                    if (answer.Upvoters.Any(v => v.Id == currentUser.Id))
                    {
                        answer.Votes -= 1;
                        answer.CreatedBy.Reputation -= 5;
                        answer.Upvoters.Remove(currentUser);
                    }
                    else if (!answer.Downvoters.Any(v => v.Id == currentUser.Id))
                    {
                        answer.Votes -= 1;
                        answer.CreatedBy.Reputation -= 5;
                        answer.Downvoters.Add(currentUser);
                    }
                    else
                    {
                        return Redirect($"~/questions/details/{answer.QuestionId}");
                    }
                }
                else
                {
                    return Redirect($"~/questions/details/{answer.QuestionId}");
                }

                answer.UpdatedAt = DateTime.Now;

                await DbContext.SaveChangesAsync();

                return Redirect($"~/questions/details/{answer.QuestionId}");
            }
        }


        [HttpPost, Authorize]
        public async Task<IActionResult> MarkAsCorrect(string id)
        {
            Answer answer = await DbContext
                                    .Answers
                                    .Include(a => a.Question)
                                    .ThenInclude(q => q.Answers)
                                    .FirstOrDefaultAsync(a => a.Id == id);

            if(answer == null)
            {
                return Redirect("~/questions/index");
            }

            User currentUser = await UserManager.GetUserAsync(User);

            if (answer.Question.CreatedById != currentUser.Id)
            {
                return Redirect($"~/questions/details/{answer.QuestionId}");
            }

            Question question = answer.Question;

            bool isAnotherQuestionMarkedAsCorrect = question.Answers.Any(a => a.MarkedAsCorrect == true);

            if (isAnotherQuestionMarkedAsCorrect)
            {
                List<Answer> alreadyMarkedAnswers = question.Answers.Where(a => a.MarkedAsCorrect == true).ToList();

                foreach (Answer markedAnswer in alreadyMarkedAnswers)
                {
                    markedAnswer.MarkedAsCorrect = false;
                }
            }

            answer.MarkedAsCorrect = true;

            await DbContext.SaveChangesAsync();

            return Redirect($"~/questions/details/{answer.QuestionId}");
        }
    }
}
