using System.ComponentModel.DataAnnotations.Schema;

namespace stack_overload.Models
{
    public class Answer
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedById { get; set; }
        
        [InverseProperty("Answers")]
        public virtual User CreatedBy { get; set; }
        public string QuestionId { get; set; }

        [InverseProperty("Answers")]
        public virtual Question Question { get; set; }

        [InverseProperty("Answer")]
        public virtual List<Comment> Comments { get; set; }

        public Answer(string body, string createdById, string questionId)
        {
            Id = Guid.NewGuid().ToString();
            Body = body;
            CreatedAt = DateTime.Now;

            CreatedById = createdById;
            QuestionId = questionId;

            Comments = new List<Comment>();
        }
    }
}
