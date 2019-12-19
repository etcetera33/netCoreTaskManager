namespace Models
{
    public class QueryParamethers
    {
        public int ItemsPerPage { get; set; } = 10;
        public int Page { get; set; } = 1;
        public string Search { get; set; } = "";
    }
}
