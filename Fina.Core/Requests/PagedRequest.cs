using Fina.Api;

namespace Fina.Core.Requests
{
    public abstract class PagedRequest : Request
    {
        public int PageSize { get; set; } = Configuration.DefaultSize;
        public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    }
}
