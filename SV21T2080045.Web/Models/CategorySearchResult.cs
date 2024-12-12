using _21T1080045.DomainModels;

namespace SV21T2080045.Web.Models
{
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
}
