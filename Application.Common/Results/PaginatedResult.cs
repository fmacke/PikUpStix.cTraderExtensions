namespace Application.Common.Results
{
    public class PaginatedResult<T> : Result
    {
        public List<T> Data { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }

        public long TotalCount { get; set; }

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;

        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        internal PaginatedResult(bool succeeded, List<T> data = null, List<string> messages = null, long count = 0L, int page = 1, int pageSize = 10)
        {
            Data = data;
            Page = page;
            base.Succeeded = succeeded;
            TotalPages = (int)Math.Ceiling((double)count / (double)pageSize);
            TotalCount = count;
        }

        public static PaginatedResult<T> Failure(List<string> messages)
        {
            return new PaginatedResult<T>(succeeded: false, null, messages, 0L);
        }

        public static PaginatedResult<T> Success(List<T> data, long count, int page, int pageSize)
        {
            return new PaginatedResult<T>(succeeded: true, data, null, count, page, pageSize);
        }
    }
}
