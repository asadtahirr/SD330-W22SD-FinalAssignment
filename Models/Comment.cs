using System.ComponentModel.DataAnnotations.Schema;

namespace stack_overload.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedById { get; set; }
        
        [InverseProperty("Comments")]
        public virtual User CreatedBy { get; set; }
        public string? QuestionId { get; set; }

        [InverseProperty("Comments")]
        public virtual Question Question { get; set; }

        public string? AnswerId { get; set; }

        [InverseProperty("Comments")]
        public virtual Answer Answer { get; set; }

        public Comment(string body, string createdById, string questionId)
        {
            Id = Guid.NewGuid().ToString();
            Body = body;
            CreatedAt = DateTime.Now;
            CreatedById = createdById;
            QuestionId = questionId;
        }
    }
}
