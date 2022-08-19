namespace stack_overload.Models.ViewModels
{
    public class QuestionIndexViewModel
    {
        public int CurrentPage { get; set; }
        public bool SortedByDate { get; set; }
        public string? TagForSorting { get; set; }
        public List<QuestionSummaryViewModel> SummaryOfQuestions;
    }
}
