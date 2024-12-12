using _21T1080045.BusinessLayers;
using _21T1080045.DomainModels;
using LiteCommerce.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T2080045.Web.Models;
using System.Buffers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T2080045.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 30;
        private const string SHIPPER_SEARCH_CONDITION = "ShipperSearchCondition";
        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(condition);
        }
        public IActionResult Search(PaginationSearchInput condition)
        {
            int rowCount;
            var data = CommonDataService.ListOfShippers(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            ShipperSearchResult model = new ShipperSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(SHIPPER_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung shipper";
            var data = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit",data);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật shipper";
            var data = CommonDataService.GetShipper(id);
            if (data == null) 
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        [HttpPost]
        public IActionResult Save(Shipper shipper)
        {
            ViewBag.Title = shipper.ShipperID == 0 ? "Bổ sung shipper" : "Cập nhật shipper";
            //Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(shipper.ShipperName))
            {
                ModelState.AddModelError(nameof(shipper.ShipperName), "Tên shipper không được để trống");
            }
            if (string.IsNullOrWhiteSpace(shipper.Phone))
            {
                ModelState.AddModelError(nameof(shipper.Phone), "Số điện thoại không được để trống");
            }
            // Kiểm tra Model có tồn tại lỗi hay không
            if (ModelState.IsValid == false)
            {
                return View("Edit", shipper);
            }

            if (shipper.ShipperID == 0)
            {
                int id = CommonDataService.AddShipper(shipper);
                if(id <= 0)
                {
                    ModelState.AddModelError(nameof(shipper.Phone), "Số điện thoại đã tồn tại");
                    return View("Edit",shipper);
                }
            }
            else 
            {
                bool result = CommonDataService.UpdateShipper(shipper);
                if (!result)
                {
                    ModelState.AddModelError(nameof(shipper.Phone), "Số điện thoại đã tồn tại");
                    return View("Edit", shipper);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if(Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetShipper(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }

    }
}
