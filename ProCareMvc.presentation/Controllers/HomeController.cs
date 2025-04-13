using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProCareMvc.business;
using ProCareMvc.business.InterfaceReposatory;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;
using ProCareMvc.presentation.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace ProCareMvc.presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmailServices emailServices;

        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork,IMapper mapper, EmailServices emailServices )
        {
            _logger = logger;

            UnitOfWork = unitOfWork;

            Mapper = mapper;
        }

     
        public async Task<IActionResult> Index()
        {
            var doctors = await UnitOfWork.Doctor.GetAll()
                .AsNoTracking()
                .Include(d => d.User)
                .Include(d => d.Department)
                .Where(d => !d.IsDeleted)
                .Select(d => new ShowDoctorInHomePageVM
                {
                    Id = d.Id,
                    FullName =  $"{d.User.FirstName} {d.User.LastName}",
                    DepartmentName =  d.Department.Name ,
                    YearsOfExperience = d.YearsOfExperience,
                    PhoneNumber =d.User.PhoneNumber ,
                    IsDeleted = d.IsDeleted,
                    ImageUrl = d.User.ImageProfileUrl 
                })
                .ToListAsync();

            return View(doctors);
            this.emailServices = emailServices;
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var doctorEntity = await UnitOfWork.Doctor.GetByIdAsync(id);

            Department d = Mapper.Map<Department>(vm);
             return View();
        }

            var doctorWithDetails = await UnitOfWork.Doctor.GetAll()
                .AsNoTracking()
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctorWithDetails == null)
            {
                return NotFound();
            }

            int startHour = 9;
            int endHour = 15;
            TimeSpan slotDuration = TimeSpan.FromMinutes(30);
            int daysAhead = 3;
            DateTime now = DateTime.Today;

      

            List<DateTime> allPossibleSlots = new List<DateTime>();

            for (int day =0 ; day < daysAhead ; day++)
            {

               DateTime currentDay = now.AddDays(day);

                for (var time = currentDay.AddHours(startHour); time < currentDay.AddHours(endHour); time = time.Add(slotDuration))

                {
                    allPossibleSlots.Add(time);
                }
            }

        public ActionResult SendEmail()
        {
            return View(new EmailViewModel());
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

        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await UnitOfWork.Order.GetAll().ToListAsync();
          
            return View(orders);
        }

        public async Task<IActionResult> GetMyOrdersById(Guid Id)
        {
            var order = await UnitOfWork.Order.GetAll()
                .Include(x => x.Patient)
                .ThenInclude(x=> x.User)
                .ThenInclude(x => x.PatientHestories)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
