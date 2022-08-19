namespace stack_overload.Models.ViewModels
{
    public class QuestionSummaryViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string QuestionCreator { get; set; }
        public int AnswersCount { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
