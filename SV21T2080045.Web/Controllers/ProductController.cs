using _21T1080045.BusinessLayers;
using _21T1080045.DomainModels;
using LiteCommerce.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T2080045.Web.Models;
using System.Net.WebSockets;

namespace SV21T2080045.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 30;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
        public IActionResult Index()
        {
            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    MinPrice = 0,
                    MaxPrice = 0,
                };

            }
            return View(condition);
        }
        public IActionResult Search(ProductSearchInput condition)
        {
            int rowCount;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize,condition.SearchValue, condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);
            ProductSearchResult model = new ProductSearchResult() 
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                MinPrice = condition.MinPrice,
                MaxPrice = condition.MaxPrice,
                RowCount = rowCount,
                Data = data 
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng ";
            var data = new Product()
            {
                ProductID = 0
            };
            return View("Edit", data);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật mặt hàng ";
            var data = ProductDataService.GetProduct(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult Save(Product data, IFormFile? _Photo) 
        {
            // xử lý ảnh
            if (_Photo != null)
            {
                string fileName = $"{DateTime.Now.Ticks}-{_Photo.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images/products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    _Photo.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(data.ProductName))
            {
                ModelState.AddModelError(nameof(data.ProductName), "Tên sản phẩm không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.ProductDescription))
            {
                ModelState.AddModelError(nameof(data.ProductDescription), "Mô tả sản phẩm không được để trống");
            }
            if (data.CategoryID == 0)
            {
                ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
            }
            if (data.SupplierID == 0)
            {
                ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng chọn nhà cung cấp");
            }
            if (string.IsNullOrWhiteSpace(data.Unit))
            {
                ModelState.AddModelError(nameof(data.Unit), "Vui lòng nhập đơn vị tính");
            }
            if (data.Price == 0)
            {
                ModelState.AddModelError(nameof(data.Price), "Vui lòng nhập giá hàng");
            }
            // Kiểm tra Model có tồn tại lỗi hay không
            if (ModelState.IsValid == false)
            {
                return View("Edit", data);
            }

            if(data.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(data);
            }
            else
            {
                bool result = ProductDataService.UpdateProduct(data);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var data = ProductDataService.GetProduct(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    var data = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id,
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                    var productPhoto = ProductDataService.GetProductPhoto(id, photoId);
                    if (productPhoto == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(productPhoto);
                case "delete":
                    bool result = ProductDataService.DeleteProductPhoto(id, photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? _Photo) 
        {
            // xử lý ảnh
            if (_Photo != null)
            {
                string fileName = $"{DateTime.Now.Ticks}-{_Photo.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images/productphotos", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    _Photo.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            if (data.PhotoID == 0)
            {
                long id = ProductDataService.AddProductPhoto(data);
            }
            if (data.PhotoID != 0) 
            {
                bool result = ProductDataService.UpdateProductPhoto(data);
            }
            return RedirectToAction("Edit", new { id = data.ProductID }); 
        }
        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    var data = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của mặt hàng";
                    var attribute = ProductDataService.GetProductAttribute(attributeId);
                    if(attribute == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(attribute); 
                case "delete":
                    bool result = ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            if(data.AttributeID == 0)
            {
                long id = ProductDataService.AddProductAttribute(data); 
            }
            else
            {
                bool result = ProductDataService.UppdateAttribute(data);
            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }
    }
        
}
