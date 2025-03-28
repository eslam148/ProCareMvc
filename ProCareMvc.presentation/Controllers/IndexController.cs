using Microsoft.AspNetCore.Mvc;

namespace ProCareMvc.presentation.Controllers
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
