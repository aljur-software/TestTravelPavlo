namespace Domain.Paging.Filters
{
    public class PaginationFilter
    {
        public const int MIN_PAGE_SIZE = 1;
        public const int MAX_PAGE_SIZE = 100;

        private int _pageNumber;
        public int PageNumber
        {

            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < MIN_PAGE_SIZE)
                {
                    _pageSize = MIN_PAGE_SIZE;
                }
                else if (value > MAX_PAGE_SIZE)
                {
                    _pageSize = MAX_PAGE_SIZE;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}