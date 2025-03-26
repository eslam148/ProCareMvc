using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.presentation.ViewModels;
using System;
using System.Threading.Tasks;

namespace ProCareMvc.presentation.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleController(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
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
                        return RedirectToAction("Index");
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
    }
}
