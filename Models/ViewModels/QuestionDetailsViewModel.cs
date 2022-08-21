namespace stack_overload.Models.ViewModels
{
    public class QuestionDetailsViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Votes { get; set; }
        public string CurrentUserId { get; set; }
        public string QuestionCreatorId { get; set; }
        public string QuestionCreatorName { get; set; }
        public int QuestionCreatorReputation { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
