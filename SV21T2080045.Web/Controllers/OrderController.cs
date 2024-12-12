using _21T1080045.BusinessLayers;
using _21T1080045.DomainModels;
using LiteCommerce.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T2080045.Web.Models;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T2080045.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR}")]
    public class OrderController : Controller
    {
        public const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";
        public const int PAGE_SIZE = 20;

        private const int PRODUCT_PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH_CONDITIONS = "ProductSearchConditions";

        private const string SHOPPING_CART = "ShoppingCart";
        public IActionResult Index()
        {
            var condition = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH_CONDITION);
            if (condition == null) 
            {
                var cultureInfo = new CultureInfo("en-GB");
                condition = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    TimeRange = $"{DateTime.Today.AddYears(-3).ToString("dd/MM/yyyy", cultureInfo)} - {DateTime.Today.ToString("dd/MM/yyyy", cultureInfo)}"
                };
            }
            return View(condition);
        }
        public IActionResult Search(OrderSearchInput condition)
        {
            int rowCount;
            var data = OrderDataService.ListOrders(out rowCount, condition.Page, condition.PageSize, condition.Status, condition.FromTime, condition.ToTime, condition.SearchValue ?? "");
            var model = new OrderSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                Status = condition.Status,
                TimeRange = condition.TimeRange,
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(ORDER_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            var condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITIONS);
            if (condition == null) 
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PRODUCT_PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(condition);
        }
        public IActionResult SearchProduct(OrderSearchInput condition) 
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            var model = new ProductSearchResult() 
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITIONS, condition);
            return View(model);
        }
        private List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return shoppingCart;
        }

        public IActionResult AddToCart(CartItem item)
        {
            if (item.SalePrice < 0 || item.Quantity <= 0)
            {
                return Json("Giá bán và số lượng không hợp lệ");
            }
            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == item.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(item);
            }
            else
            {
                existsProduct.Quantity += item.Quantity;
                existsProduct.SalePrice = item.SalePrice;
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
            {
                shoppingCart.RemoveAt(index);
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult ShoppingCart()
        {
            return View(GetShoppingCart());
        }

        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public ActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel() 
            {
                Order = order,
                Details = details
            };
            return View(model);
        }
        public IActionResult Init(int customerID = 0, string deliveryProvince = "", string deliveryAddress = "")
        {
            var shoppingCart = GetShoppingCart();
            if (shoppingCart.Count == 0)
            {
                return Json("Giỏ hàng trống. Vui lòng chọn mặt hàng cần bán");
            }

            if (customerID == 0 || string.IsNullOrWhiteSpace(deliveryProvince) || string.IsNullOrWhiteSpace(deliveryAddress))
            {
                return Json("Vui lòng nhập đầy đủ thông tin khách hàng và nơi giao hàng");
            }

            var userData = User.GetUserData();
            int employeeID = 0;
            if (userData != null)
            {
                employeeID = int.Parse(userData.UserID);
            }

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCart)
            {
                orderDetails.Add(new OrderDetail()
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    SalePrice = item.SalePrice
                });
            }
            int orderID = OrderDataService.InitOrder(employeeID, customerID, deliveryProvince, deliveryAddress, orderDetails);
            ClearCart();
            return Json(orderID);
        }

        public ActionResult EditDetail(int id = 0, int productId = 0)
        {
            var data = OrderDataService.GetOrderDetail(id, productId);
            if (data == null) 
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public ActionResult DeleteDetail(int id = 0, int productID = 0)
        {
            bool result = OrderDataService.DeleteOrderDetail(id, productID);
            if (result)
            {
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDetail(OrderDetail data)
        {

            bool result = OrderDataService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            if (result) 
            {
                return RedirectToAction("Details" , new { id = data.OrderID });
            }
            return RedirectToAction("Index");
        }
        public ActionResult Shipping(int id = 0)
        {
            return View(id);
        }
        public ActionResult SaveShipping(int orderID, int shipperID = 0) 
        {
            if (shipperID == 0)
            {
                TempData["Error"] = "Chuyển giao hàng thất bại, Vui lòng kiểm tra lại !";
                return RedirectToAction("Details", new { id = orderID });
            }
            bool result = OrderDataService.ShipOrder(orderID, shipperID);
            if (result)
            {
                TempData["Success"] = "Chuyển giao hàng thành công !";
                return RedirectToAction("Details", new { id = orderID });
            }
            TempData["Error"] = "Chuyển giao hàng thất bại, Vui lòng kiểm tra lại !";
            return RedirectToAction("Details", new { id = orderID });
        }
        public ActionResult Accept(int id = 0) 
        {
            bool result = OrderDataService.AcceptOrder(id);
            if (result)
            {
                TempData["Success"] = "Duyệt đơn hàng thành công !";
                return RedirectToAction("Details", new { id = id });
            }
            TempData["Error"] = "Duyệt đơn hàng thất bại, vui lòng kiểm tra lại !";
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Finish(int id = 0) 
        {
            bool result = OrderDataService.FinishOrder(id);
            if (result)
            {
                TempData["Success"] = "Xác nhận hoàn tất đơn hàng thành công !";
                return RedirectToAction("Details", new { id = id });
            }
            TempData["Error"] = "Không thể xác nhận, vui lòng kiểm tra lại !";
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Cancel(int id = 0)
        {
            bool result = OrderDataService.CancelOrder(id);
            if (result)
            {
                TempData["Success"] = "Hủy đơn hàng thành công !";
                return RedirectToAction("Details", new { id = id });
            }
            TempData["Error"] = "Hủy đơn hàng thất bại, vui lòng kiểm tra lại !";
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Reject(int id = 0)
        {
            bool result = OrderDataService.RejectOrder(id);
            if (result)
            {
                TempData["Success"] = "Từ chối hàng thành công !";
                return RedirectToAction("Details", new { id = id });
            }
            TempData["Error"] = "Từ chối đơn hàng thất bại, vui lòng kiểm tra lại !";
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Delete(int id = 0)
        {
            bool result = OrderDataService.DeleteOrder(id);
            if (!result)
            {
                TempData["Error"] = "Xóa đơn hàng thất bại, vui lòng kiểm tra lại !";
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Index");
        }
    }
}
