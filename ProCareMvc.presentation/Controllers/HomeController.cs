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
using System.Security.Claims;


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
            //this.emailServices = emailServices;
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var doctorEntity = await UnitOfWork.Doctor.GetByIdAsync(id);

            if (doctorEntity == null || doctorEntity.IsDeleted)
            {
                return NotFound("Doctor is not Avalible now");
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

            for (int day = 0; day < daysAhead; day++)
            {

                DateTime currentDay = now.AddDays(day);

                for (var time = currentDay.AddHours(startHour); time < currentDay.AddHours(endHour); time = time.Add(slotDuration))

                {
                    allPossibleSlots.Add(time);
                }
            }
            List<AppointmentVM> bookedAppointments = await UnitOfWork.Appointment
                   .GetAll()
                  .AsNoTracking()
                   .Where(a => a.DoctorId == id && a.StartTime >= now && a.StartTime < now.AddDays(daysAhead) && !a.IsDeleted)
                   .Select(a => new AppointmentVM { StartTime = a.StartTime, EndTime = a.EndTime })
                   .ToListAsync();



            List<AppointmentVM> availableAppointments = allPossibleSlots
                .Where(slot => !bookedAppointments.Any(booked => slot >= booked.StartTime && slot < booked.EndTime))
                .Select(slot => new AppointmentVM
                {
                    StartTime = slot,
                    EndTime = slot.Add(slotDuration),
                    Name = "Available",
                    PatientId = Guid.Empty

                })
                .ToList();




            var doctor = new ShowDoctorInHomePageVM
            {
                Id = doctorWithDetails.Id,
                FullName = $"{doctorWithDetails.User.FirstName} {doctorWithDetails.User.LastName}",
                DepartmentName = doctorWithDetails.Department.Name,
                YearsOfExperience = doctorWithDetails.YearsOfExperience,
                PhoneNumber = doctorWithDetails.User.PhoneNumber,
                IsDeleted = doctorWithDetails.IsDeleted,
                ImageUrl = doctorWithDetails.User.ImageProfileUrl,
                AvailableAppointments = availableAppointments
            };

            return View(doctor);
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
        [Authorize]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await UnitOfWork.Order.GetAll().Where(x=>x.Patient.UserId == Guid.Parse(userId)) .ToListAsync();
          
            return View(orders);
        }

        [Authorize]
        public async Task<IActionResult> GetMyOrdersById(Guid Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await UnitOfWork.Order.GetAll()
                .Include(x => x.Patient)
                .ThenInclude(x=> x.User)
                .ThenInclude(x => x.PatientHestories)
                .FirstOrDefaultAsync(x => x.Id == Id && x.Patient.UserId == Guid.Parse(userId));
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
