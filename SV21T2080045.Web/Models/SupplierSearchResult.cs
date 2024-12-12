using _21T1080045.DomainModels;

namespace SV21T2080045.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}
