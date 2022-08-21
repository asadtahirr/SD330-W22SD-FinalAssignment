using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace stack_overload.Models
{
    public class User : IdentityUser
    {
        [InverseProperty("CreatedBy")]
        public virtual List<Question> Questions { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual List<Answer> Answers { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual List<Comment> Comments { get; set; }

        [InverseProperty("Upvoters")]
        public virtual List<Question> UpvotedQuestions { get; set; }

        [InverseProperty("Downvoters")]
        public virtual List<Question> DownvotedQuestions { get; set; }

        [InverseProperty("Upvoters")]
        public virtual List<Answer> UpvotedAnswers { get; set; }

        [InverseProperty("Downvoters")]
        public virtual List<Answer> DownvotedAnswers { get; set; }
        public int Reputation { get; set; } = 0;

        public User()
        {
            Questions = new List<Question>();
            Answers = new List<Answer>();
            Comments = new List<Comment>();
            UpvotedQuestions = new List<Question>();
            DownvotedQuestions = new List<Question>();
            UpvotedAnswers = new List<Answer>();
            DownvotedAnswers = new List<Answer>();
        }
    }
}
