using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.Database.Entity;

namespace ProCareMvc.presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager; // استخدام User بدلاً من ApplicationUser
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);

        //    }


        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        RedirectToAction("Index", "Home");
        //    }


        //    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        //    if (result.Succeeded)
        //    {

        //        await _signInManager.RefreshSignInAsync(user);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //    return View(model);



        //}

    }
}
