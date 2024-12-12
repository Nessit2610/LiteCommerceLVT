using _21T1080045.BusinessLayers;
using _21T1080045.DataLayers.SQLServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SV21T2080045.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;
            //kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error","Vui lòng nhập thông tin đăng nhập");
                return View();
            }
            // TODO : Kiểm tra thông tin đăng nhập
            var userAccount = UserAccountService.Authorize(UserAccountService.UserTypes.Employee, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập không thành công");
                return View();
            }
            //Đăng nhập thành công : ghi nhận trạng thái
            //1. Tạo ra thông tin của người dùng
            var userData = new WebUserData()
            {
                UserID = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
                Roles = userAccount.RoleNames.Split(',').ToList()
            };
            //2. Ghi nhận trạng thái đăng nhập
            await HttpContext.SignInAsync(userData.CreatePrincipal());

            //3. Quay về trang chủ
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenined()
        {
            return View();
        }
        public IActionResult ChangePassword(int id)
        {
            return View(id);
        }
        public IActionResult SavePassword(int employeeID, string oldPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                TempData["Error"]= "Xác nhận mật khẩu không khớp !";
                return RedirectToAction("ChangePassword", new { id = employeeID });
            }
            bool result = false;
            result = UserAccountService.ChangePassword(oldPassword,employeeID,newPassword);
            if (!result) 
            {
                TempData["Error"] = "Mật Khẩu cũ không đúng!";
                return RedirectToAction("ChangePassword", new { id = employeeID });
            }
            TempData["Success"] = "Đổi mật khẩu thành công hàng thành công !";
            return RedirectToAction("ChangePassword", new { id = employeeID });
            
        }
    }
}
