using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;

namespace ProCareMvc.presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);

            }


            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Index", "Home");
            }


            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {

                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);



        }

        //[HttpPost]
        //public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        User user= await  _userManager.FindByEmailAsync(model.Email);
        //        if (user != null)
        //        {

        //        }

        //    }


















        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No user found with this email.");
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            await _emailService.SendEmailAsync(model.Email, "Reset your password", $"Click here to reset: {resetPasswordLink}");

            ViewBag.Message = "If the email exists, a reset link has been sent.";
            return View(model);
        }






    }
}




