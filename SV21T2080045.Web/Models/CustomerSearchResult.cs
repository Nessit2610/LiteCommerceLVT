using _21T1080045.DomainModels;

namespace SV21T2080045.Web.Models
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
}
