using System.ComponentModel.DataAnnotations.Schema;

namespace stack_overload.Models
{
    public class Tag
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Tags")]
        public virtual List<Question> Questions { get; set; }

        public Tag(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;

            Questions = new List<Question>();
        }
    }
}
