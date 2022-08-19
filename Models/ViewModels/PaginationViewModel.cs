namespace stack_overload.Models.ViewModels
{
    public class PaginationViewModel
    {
        public int PageNumber { get; set; }
        public static int QuestionsPerPage { get; } = 10;
    }
}
