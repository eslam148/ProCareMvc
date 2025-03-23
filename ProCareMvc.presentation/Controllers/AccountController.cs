using Microsoft.AspNetCore.Mvc;

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
