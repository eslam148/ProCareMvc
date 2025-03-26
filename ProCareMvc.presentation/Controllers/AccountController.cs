using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ProCareMvc.business;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;
using System.IO;

namespace ProCareMvc.presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManeg;
        private readonly IUnitOfWork unitOfWork;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManeg , IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManeg = signInManeg;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM userFromReq)
        {
            if (ModelState.IsValid)
            {
                // 1) add database
                User user = new User();
                user.Email = user.UserName = userFromReq.Email;

                IdentityResult result =  await userManager.CreateAsync(user, userFromReq.Password);
                if (result.Succeeded)
                {
                    // 2) Add To cookies
                    await userManager.AddToRoleAsync(user, "Admin");
                    await signInManeg.SignInAsync(user, false );    //create cookies => Login
                    return RedirectToAction("Login" , "Account");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", userFromReq);
        }

        public IActionResult PatientRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRegister(PatientRegisterVM PatientFromReq)
        {
            if (ModelState.IsValid)
            {
                // 1) add database
                User user = new User();
                
                user.FirstName  = PatientFromReq.FirstName;
                user.LastName = PatientFromReq.LastName;
                user.Email = user.UserName = PatientFromReq.Email;
                user.PhoneNumber = PatientFromReq.PhoneNumber;
                user.BirthDate  = PatientFromReq.BirthDate;
                user.Gender = PatientFromReq.Gender;



                IdentityResult result = await userManager.CreateAsync(user, PatientFromReq.Password);
                if (result.Succeeded)
                {
                    Patient patient = new Patient();
                    patient.UserId = user.Id;

                    // 2) Add To cookies
                    //await userManager.AddToRoleAsync(userFromDb, "Admin");
                    //await signInManeg.SignInAsync(user, false);    //create cookies
                    await unitOfWork.Patient.InsertAsync(patient);
                    return RedirectToAction("Login", "Account");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", PatientFromReq);
        }
        

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientLogin(LoginVM userFromReq)
        {
            if (ModelState.IsValid)
            {
                User? userFromDb = await userManager.FindByEmailAsync(userFromReq.Email);
                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, userFromReq.Password);
                    if (found)
                    {
                        await signInManeg.SignInAsync(userFromDb, userFromReq.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid email or password");
            }
            return View("Login",userFromReq);
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManeg.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
