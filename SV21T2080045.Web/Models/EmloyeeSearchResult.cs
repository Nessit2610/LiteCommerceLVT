using _21T1080045.DomainModels;

namespace SV21T2080045.Web.Models
{
    public class EmloyeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }
}
