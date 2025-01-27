namespace MPath.Application.Shared.Responses
{
    public class PaginationDto<T>
    {
        public int TotalPage { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public T Data { get; set; }
        
        public PaginationDto(T data, int totalPage, int page, int pageSize)
        {
            Data = data;
            TotalPage = totalPage;
            Page = page;
            PageSize = pageSize;
        }
        public PaginationDto()
        {
        }
    }
    
}
