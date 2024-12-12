using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SV21T2080045.Web
{
    /// <summary>
    /// Lưu giữ thông tin người dùng được ghi trong cookie
    /// </summary>
    public class WebUserData
    {
        public string UserID { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Photo { get; set; } = "";
        public List<string>? Roles { get; set; }

        /// <summary>
        /// Danh sách các Claim 
        /// </summary>
        private List<Claim> Claims
        {
            get
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(nameof(UserID),UserID),
                    new Claim(nameof(UserName), UserName),
                    new Claim(nameof(DisplayName), DisplayName),
                    new Claim(nameof(Photo),Photo)
                };
                if(Roles != null)
                {
                    foreach(var role in Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
                return claims;
            }
        }
        /// <summary>
        /// Tạo ClaimsPrincipal dựa trên thông tin của người dùng (cần lưu trong cookie) 
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal CreatePrincipal()
        {
            var identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principals = new ClaimsPrincipal(identity);
            return principals;
        }
    }
}
