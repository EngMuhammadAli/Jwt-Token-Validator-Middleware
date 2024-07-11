using Jwt_Token_Validator_Middleware.Data;
using Jwt_Token_Validator_Middleware.Models;
using Jwt_Token_Validator_Middleware.Models.ViewModel;
using Jwt_Token_Validator_Middleware.TokenServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Jwt_Token_Validator_Middleware.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly IToken _token;
        public LoginController(ApplicationDbContext applicationDbContext , IToken token)
        {
            _Context = applicationDbContext;
            _token = token;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            string Email = loginViewModel.Email;
            string Password = loginViewModel.Password;
            if (ModelState.IsValid)
            {
                User user = _Context.Users.FirstOrDefault(s => s.Email == Email && s.Password == Password);
                if (user != null)
                {
                    string Token = _token.GenerateJWT(user);
                    SetJwtTokenCookie(Token);
                    SaveToken(user.UserID , Token);
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    ModelState.AddModelError("","User Not Exsist");
                    return View(loginViewModel);
                }

            }
            return View();
        }
        private void SetJwtTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                Secure = true, // Ensure to set Secure to true if using HTTPS
                SameSite = SameSiteMode.None // Adjust SameSite as needed based on your application's requirements
            };

            Response.Cookies.Append("Authorization", token, cookieOptions);
        }

        public void SaveToken(int UserId ,string Token)
        {
            var usertoken = new UserToken()
            {
                UserID = UserId,
                Token = Token
            };
            _Context.UserTokens.Add(usertoken);
            _Context.SaveChanges();
        }
    }
}
