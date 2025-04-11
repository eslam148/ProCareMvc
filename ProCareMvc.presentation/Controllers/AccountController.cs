using Microsoft.AspNetCore.Authentication.Cookies;
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
                var token =   await  _userManager.GeneratePasswordResetTokenAsync(user);
                var result1 = await _userManager.ResetPasswordAsync(user, token, model.Password);
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM userFromReq)
    {
        if (ModelState.IsValid)
        {
            // 1) add database
            User user = new User();
            user.Email = user.UserName = userFromReq.Email;

            IdentityResult result = await userManager.CreateAsync(user, userFromReq.Password);
            if (result.Succeeded)
            {
                // 2) Add To cookies
                await userManager.AddToRoleAsync(user, "Admin");
                await _signInManager.SignInAsync(user, false);    //create cookies => Login
                return RedirectToAction("Login", "Account");
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

            user.FirstName = PatientFromReq.FirstName;
            user.LastName = PatientFromReq.LastName;
            user.Email = user.UserName = PatientFromReq.Email;
            user.PhoneNumber = PatientFromReq.PhoneNumber;
            user.BirthDate = PatientFromReq.BirthDate;
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

    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PatientLogin(LoginVM userFromReq)
    {
        if (ModelState.IsValid)
        {
            User? userFromDb = await userManager.FindByEmailAsync(userFromReq.Email);
            if (userFromDb != null)
            {
                //var token = await _userManager.GeneratePasswordResetTokenAsync(userFromDb);
                //var result1 = await _userManager.ResetPasswordAsync(userFromDb, token, userFromReq.Password);
                bool found = await userManager.CheckPasswordAsync(userFromDb, userFromReq.Password);
                if (found)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim("ProfileImage", userFromDb.ImageProfileUrl ?? "doc-5.jpg"));
                    var principal = new ClaimsPrincipal(identity);
                    await _signInManager.SignInAsync(userFromDb, userFromReq.RememberMe);
                    HttpContext.User = principal;
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid email or password");
        }
        return View("Login", userFromReq);
    }
    public IActionResult ForgetPassword()
    {
        return View();
    }

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

        //  await _emailService.SendEmailAsync(model.Email, "Reset your password", $"Click here to reset: {resetPasswordLink}");

        ViewBag.Message = "If the email exists, a reset link has been sent.";
        return View(model);
    }
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }


}
