namespace Domain.Paging
{
    public class PagingModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalEntries { get; set; }
        public int TotalPages { get; set; }
    }
}
