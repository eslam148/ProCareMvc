using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProCareMvc.business;
using ProCareMvc.business.InterfaceReposatory;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;
using ProCareMvc.presentation.Services;

namespace ProCareMvc.presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmailServices emailServices;

        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork,IMapper mapper, EmailServices emailServices)
        {
            _logger = logger;

            UnitOfWork = unitOfWork;

            Mapper = mapper;
            this.emailServices = emailServices;
        }

        public async Task<IActionResult> Index(DepartmentVM vm)
        {

            Department d = Mapper.Map<Department>(vm);
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

        public ActionResult SendEmail()
        {
            return View(new EmailViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(EmailViewModel model)
        {
            
                emailServices.sendEmail(model.To,"Test Email", model.Body);
                ViewBag.Message = "Email sent successfully!";
            
            return View(model);
        }
    }
}
