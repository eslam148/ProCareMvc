using Microsoft.AspNetCore.Mvc;

namespace ProCareMvc.presentation.Controllers
{
    public class ShowDoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
