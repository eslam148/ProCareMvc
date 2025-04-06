using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ProCareMvc.business;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;
using System.IO;
using ProCareMvc.Database.Entity;

namespace ProCareMvc.presentation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
