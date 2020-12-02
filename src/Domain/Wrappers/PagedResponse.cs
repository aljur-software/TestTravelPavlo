using Domain.Paging;

namespace Domain.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public PagingModel Paging { get; set; }

        public PagedResponse(T data, PagingModel paging) : base(data)
        {
            Paging = paging;
        }
    }
}
