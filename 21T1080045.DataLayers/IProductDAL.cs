using _21T1080045.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.DataLayers
{
    public interface IProductDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang)</param>
        /// <param name="searchValue">Tên mặt hàng cần tìm (chuỗi rỗng nếu không tìm kiếm)</param>
        /// <param name="categoryID">Mã loại hàng cần tìm (0 nếu không tìm theo loại hàng)</param>
        /// <param name="supplierID">Mã nhà cung cấp (0 nếu không tìm theo mã cung cấp)</param>
        /// <param name="minPrice">Mức giá nhỏ nhất trong khoảng giá cần tìm</param>
        /// <param name="maxPrice">Mức giá lớn nhất trong khoảng giá cần tìm (0 nếu không hạn chế mức giá)</param>
        /// <returns></returns>
        List<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0 );

        //Đếm số lượng mặt hàng tìm kiếm được
        int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);

        //Lấy thông tin mặt hàng theo mã
        Product? Get(int productID);

        //Bổ sung mặt hàng mới, hàm trả về mã của mặt hàng được bổ sung
        int Add(Product data);

        //Cập nhật thông tin mặt hàng
        bool Update(Product data);

        //Xóa mặt hàng
        bool Delete(int productID);

        //Kiểm tra mặt hàng có đơn hàng liên quan hay không
        bool InUsed(int productID);

        //Lấy danh sách ảnh của mặt hàng (sắp xếp theo thứ tự của DisplayOrder)
        IList<ProductPhoto> ListPhotos(int productID);

        //Lấy thông tin của một ảnh dựa bào ID
        ProductPhoto? GetPhoto(long productID, long photoID);

        //Bổ sung 1 ảnh cho mặt hàng (hàm trả về mã của ảnh được bổ sung)
        long AddPhoto(ProductPhoto data);

        //Cập nhật ảnh của mặt hàng
        bool UpdatePhoto(ProductPhoto data);

        //Xóa ảnh của mặt hàng
        bool DeletePhoto(long productID, long photoID);

        //lấy danh sách các thuộc tính của mặt hàng, sắp xếp theo thứ tự của DisplayOrder
        IList<ProductAttribute> ListAttributes(int productID);

        //lấy thông tin của thuộc tính theo mã thuộc tính
        ProductAttribute? GetAttribute(long attributeID);
        
        //Bổ sung thuộc tính cho mặt hàng
        long AddAttribute(ProductAttribute data);

        //cập nhật thuộc tính của mặt hàng
        bool UpdateAttribute(ProductAttribute data);

        //xóa thuộc tính
        bool DeleteAttribute(long attributeID);
    }
}
