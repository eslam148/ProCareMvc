using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.business;
using ProCareMvc.presentation.Models;

namespace ProCareMvc.presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IUnitOfWork UnitOfWork { get; }

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            UnitOfWork.OrderItemRepository.GetById(1);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
