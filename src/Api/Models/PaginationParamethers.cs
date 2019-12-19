namespace Models
{
    public class PaginationParamethers
    {
        public int ItemsPerPage { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int ItemsCount { get; set; }
        public int PageCount { get; set; }
        public int PagesOffset { get; set; }
    }
}
