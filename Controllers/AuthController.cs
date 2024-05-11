using Google.Authenticator;
using LoginWith2FA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LoginWith2FA.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }


        // Login
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username == "testUser" && password == "123")
            {
                return RedirectToAction("TwoFactorLogin");
            }

            ViewBag.ErrorMessage = "username or password incorrect";
            return View();
        }


        // TwoFactorAuth
        public ActionResult TwoFactorLogin()
        {
            var model = new AuthenticatorVM();

            var user = "testUser"; //_dbContext.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var TwoFactorSecretCode = _config["TwoFactorSecretCode"];
            var accountSecretKey = $"{TwoFactorSecretCode}-{user}";
            var setupCode = twoFactorAuthenticator.GenerateSetupCode("2FA.Demo.App", user,
                Encoding.ASCII.GetBytes(accountSecretKey));

            model.QrCodeSetupImageUrl = setupCode.QrCodeSetupImageUrl;
            model.Account = setupCode.Account;
            model.ManualEntryKey = setupCode.ManualEntryKey;

            return View(model);
        }


        [HttpPost]
        public ActionResult TwoFactorLogin(AuthenticatorVM model)
        {
            var user = "testUser";
            var twoFactorSecretCode = _config["TwoFactorSecretCode"];
            var accountSecretKey = $"{twoFactorSecretCode}-{user}";
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var result = twoFactorAuthenticator
                .ValidateTwoFactorPIN(accountSecretKey, model.UserInputCode, TimeSpan.FromSeconds(30));

            if (!result)
            {
                ViewBag.Message = "Faulty Code";
                return View(model);
            }

            // Set session data
            HttpContext.Session.SetString("LoginResult", "success"); //dummy object
            return RedirectToAction("Index", "Home");
        }
    }
}
