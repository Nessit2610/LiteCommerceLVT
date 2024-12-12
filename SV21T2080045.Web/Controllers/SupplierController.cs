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
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 30;
        private const string SUPPLIER_SEARCH_CONDITION = "SupplierSearchCondition";
        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(SUPPLIER_SEARCH_CONDITION);
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
            var data = CommonDataService.ListOfSuppliers(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            SupplierSearchResult model = new SupplierSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(SUPPLIER_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            var data = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit",data);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật nhà cung cấp";
            var data = CommonDataService.GetSupplier(id);
            if (data == null) 
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        [HttpPost]
        public IActionResult Save(Supplier supplier) 
        {
            ViewBag.Title = supplier.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
            // kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
            {
                ModelState.AddModelError(nameof(supplier.SupplierName), "Tên nhà cung cấp không được để trống");
            }
            if (string.IsNullOrWhiteSpace(supplier.ContactName))
            {
                ModelState.AddModelError(nameof(supplier.ContactName), "Tên giao dịch không được để trống");
            }
            if (string.IsNullOrWhiteSpace(supplier.Province))
            {
                ModelState.AddModelError(nameof(supplier.Province), "Tỉnh thành không được để trống");
            }
            if (string.IsNullOrWhiteSpace(supplier.Address))
            {
                ModelState.AddModelError(nameof(supplier.Address), "Địa chỉ không được để trống");
            }
            if (string.IsNullOrWhiteSpace(supplier.Phone))
            {
                ModelState.AddModelError(nameof(supplier.Phone), "Số điện thoại không được để trống");
            }
            if (string.IsNullOrWhiteSpace(supplier.Email))
            {
                ModelState.AddModelError(nameof(supplier.Email), "Email không được để trống");
            }



            // Kiểm tra Model có tồn tại lỗi hay không
            if (ModelState.IsValid == false)
            {
                return View("Edit", supplier);
            }
            try
            {
                //thực hiện action
                if (supplier.SupplierID == 0)
                {
                    int id = CommonDataService.AddSupplier(supplier);
                    if (id <= 0)
                    {
                        if(id == -1)
                        {
                            ModelState.AddModelError(nameof(supplier.Email), "Email đã được sử dụng");
                        }
                        if(id == -2)
                        {
                            ModelState.AddModelError(nameof(supplier.Phone), "Số điện thoại đã được sử dụng");
                        }
                        return View("Edit", supplier);
                    }
                }
                else
                {
                    bool result = CommonDataService.UpdateSupplier(supplier);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(supplier.Email), "Email đã được sử dụng");
                        return View("Edit", supplier);
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Error", "Hệ thống tạm thời gián đoạn");
                return View("Edit");
            }
            
        }

        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST") 
            {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetSupplier(id);
            if (data == null) 
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
    }
}
