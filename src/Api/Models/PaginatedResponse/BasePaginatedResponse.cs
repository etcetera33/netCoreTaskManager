using System.Collections.Generic;

namespace Models.PaginatedResponse
{
    public class BasePaginatedResponse<T>
    {
        public IEnumerable<T> EntityList { get; set; }
        public int PagesCount { get; set; }
    }
}
