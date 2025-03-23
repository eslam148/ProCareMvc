using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.business;
using ProCareMvc.business.InterfaceReposatory;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;

namespace ProCareMvc.presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _logger = logger;

            UnitOfWork = unitOfWork;

            Mapper = mapper;
        }

        public async Task<IActionResult> Index(DepartmentVM vm)
        {

            Department d = Mapper.Map<Department>(vm);
           await  UnitOfWork.Department.InsertAsync(d);
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
