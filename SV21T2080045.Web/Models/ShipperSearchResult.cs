using _21T1080045.DomainModels;

namespace SV21T2080045.Web.Models
{
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
}
