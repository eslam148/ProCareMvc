using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.business;
using ProCareMvc.business.Interface;
using ProCareMvc.Database.Entity;
using ProCareMvc.Database.Utility;
using ProCareMvc.presentation.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController(IUnitOfWork unitOfWork, SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager) : Controller
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager= userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

    [HttpGet]
    public IActionResult Login()
    {
        //User user = new User
        //{
        //    UserName = "Admin",
        //    PhoneNumber = "010654",
        //    BirthDate = DateOnly.FromDateTime(DateTime.Now),
        //    Email = "islam@example.com",
        //    FirstName = "Admin",
        //    LastName = "Admin",
        //    Gender = Gender.Male,

        //};
        //_userManager.CreateAsync(user, "Admin@123");
        
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

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddRoleViewModel model)
    {
        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(model.RoleName))
        {
            bool roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(model.RoleName));
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("", "Eroooooooooor");
            }
            else
            {
                ModelState.AddModelError("", "Already Exsit");
            }
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> UserProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
          var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var vm = new UserProfileViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    ImageProfileUrl = user.ImageProfileUrl,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender
                };
                return View(vm);
       
        }
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var fileName = $"{Guid.NewGuid()}_{profileImage.FileName}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }


                user.ImageProfileUrl = fileName;
               await _userManager.UpdateAsync(user);

            return Ok();
            }
        

        return BadRequest();
    }




}




