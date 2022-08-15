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

        public User()
        {
            Questions = new List<Question>();
            Answers = new List<Answer>();
            Comments = new List<Comment>();
        }
    }
}
