namespace stack_overload.Models.ViewModels
{
    public class QuestionDetailsViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
