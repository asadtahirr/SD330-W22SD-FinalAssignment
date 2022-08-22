namespace stack_overload.Models.ViewModels
{
    public class AdministrationQuestionViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedByName { get; set; }   
        public string CreatedById { get; set; } 
        public int AnswersCount { get; set; }
        public List<Tag> Tags { get; set; }

    }
}
