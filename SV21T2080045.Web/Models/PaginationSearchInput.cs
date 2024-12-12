namespace SV21T2080045.Web.Models
{
    /// <summary>
    /// Lưu giữ cấc thông tin đầu vào sử dụng cho chức năng tìm kiếm và hiển thị dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        //trang cần hiển thị
        public int Page { get; set; } = 1;
        //số dòng hiển thị trên mỗi trang
        public int PageSize {  get; set; }
        //chuỗi tìm kiếm
        public string SearchValue { get; set; } = "";
    }
}
