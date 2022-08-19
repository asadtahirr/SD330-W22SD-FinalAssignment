using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace stack_overload.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string CreatedById { get; set; }

        [InverseProperty("Questions")]
        public virtual User CreatedBy { get; set; }

        [InverseProperty("Questions")]
        public virtual List<Tag> Tags { get; set; }
        
        [InverseProperty("Question")]
        public virtual List<Answer> Answers { get; set; }

        [InverseProperty("Question")]
        public virtual List<Comment> Comments { get; set; }
        public int Votes { get; set; }

        [InverseProperty("UpvotedQuestions")]
        public virtual List<User> Upvoters { get; set; }

        [InverseProperty("DownvotedQuestions")]
        public virtual List<User> Downvoters { get; set; }

        public Question(string title, string body, string createdById)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Body = body;            
            CreatedAt = DateTime.Now;

            CreatedById = createdById;

            Tags = new List<Tag>();
            Answers = new List<Answer>();
            Comments = new List<Comment>();
            Upvoters = new List<User>();
            Downvoters = new List<User>();
        }
    }
}
