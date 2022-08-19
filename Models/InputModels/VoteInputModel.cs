using System.ComponentModel.DataAnnotations;

namespace stack_overload.Models.InputModels
{
    public class VoteInputModel
    {
        [Required]
        public string Action { get; set; }
    }
}
