namespace HPTA.Common
{
    public class PagedResult<TResult>
    {
        public IEnumerable<TResult> Items { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public PagedResult()
        {
        }

        public PagedResult(int? pageNumber, int? pageSize)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 0;
            }

            if (!pageSize.HasValue)
            {
                pageSize = 50;
            }

            PageNumber = pageNumber.Value;
            PageSize = pageSize.Value;
        }
    }
}
