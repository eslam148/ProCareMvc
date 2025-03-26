using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.business;
using ProCareMvc.business.Interface;
using ProCareMvc.Database.Entity;
using ProCareMvc.Database.Utility;
using ProCareMvc.presentation.Models;
using System;
using System.Threading.Tasks;

public class AccountController(IUnitOfWork unitOfWork, SignInManager<User> signInManager, UserManager<User> userManager) : Controller
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager= userManager;

    [HttpGet]
    public IActionResult Login()
    {
        User user = new User
        {
            UserName = "Admin",
            PhoneNumber = "010654",
            BirthDate = DateOnly.FromDateTime(DateTime.Now),
            Email = "islam@example.com",
            FirstName = "Admin",
            LastName = "Admin",
            Gender = Gender.Male,

        };
        _userManager.CreateAsync(user, "Admin@123");
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(AdminLoginViewModel model)
    {
        if (ModelState.IsValid)
        {

              var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }
}
