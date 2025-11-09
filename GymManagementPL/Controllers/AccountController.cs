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
        public async Task<ActionResult> Login(AccountViewModel accountView)
        {
            if(!ModelState.IsValid) return View(accountView);

            var user = await _accountService.ValidateAsync(accountView);
            if(user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(accountView);
            }

            var result =  await _signInManager.PasswordSignInAsync(user, accountView.Password, accountView.RememberMe, false);
            if(result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Not Allowed");

            if (result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account is Locked out");

            if(result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(accountView);
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
