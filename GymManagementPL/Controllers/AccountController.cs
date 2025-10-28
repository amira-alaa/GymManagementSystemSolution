using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel accountView)
        {
            if(!ModelState.IsValid) return View(accountView);

            var user = _accountService.Validate(accountView);
            if(user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(accountView);
            }

            var result =  _signInManager.PasswordSignInAsync(user, accountView.Password, accountView.RememberMe, false).Result;
            if(result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Not Allowed");

            if (result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account is Locked out");

            if(result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(accountView);
        }

        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
