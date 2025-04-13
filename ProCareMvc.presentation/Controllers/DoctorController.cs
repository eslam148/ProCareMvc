using Microsoft.AspNetCore.Mvc;//
using Microsoft.EntityFrameworkCore;//
using ProCareMvc.business;//
using ProCareMvc.Database.Entity;//
using ProCareMvc.presentation.Models;//
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace ProCareMvc.presentation.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult ShowPatientHistoryList()
        {
                List<PatientHestoryVM> hestoryVMs = unitOfWork.PatientHestory
                .GetAll().Include( ph => ph.User).Select(ph => new PatientHestoryVM()
                {
                    Id = ph.Id,
                    Diagnosis = ph.Diagnosis,
                    Treatment = ph.Treatment,
                    Medication = ph.Medication,
                    VisitDate = ph.VisitDate,
                    NextAppointment = ph.NextAppointment,
                    UserId = ph.UserId,
                    User = ph.User,
                }).ToList();
            return View(hestoryVMs);
        }

        // GET: ShowController/Details/5
        public async Task<IActionResult> ShowDetailsOfPatientHistory(Guid id)
        {
            PatientHestory? patnthstryFrmDB = await unitOfWork.PatientHestory.GetAll()
                .Include(ph => ph.User).SingleOrDefaultAsync( ph => ph.Id == id);
            if (patnthstryFrmDB == null)
            {
                return NotFound();
            }
            PatientHestoryVM patenthstryVM = new PatientHestoryVM
            {
                Id = patnthstryFrmDB.Id,
                Diagnosis = patnthstryFrmDB.Diagnosis,
                Treatment = patnthstryFrmDB.Treatment,
                Medication = patnthstryFrmDB.Medication,
                VisitDate = patnthstryFrmDB.VisitDate,
                NextAppointment = patnthstryFrmDB.NextAppointment,
                UserId = patnthstryFrmDB.UserId,
                User = patnthstryFrmDB.User,
            };
            return View(patenthstryVM);
        }

        [HttpGet]
        public IActionResult CreatePatientHistoryRecord()
        {
            List<Patient> patients = unitOfWork.Patient.GetAll()
                .Include(p => p.User).ToList();

            PatientHestoryVM patientHestory = new PatientHestoryVM()
            {
                Patients = patients
            };
            return View(patientHestory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatientHistoryRecord(PatientHestoryVM model)
        {
            if (ModelState.IsValid)
            {          
                PatientHestory patientHestoryToDB = new PatientHestory()
                {
                    Diagnosis = model.Diagnosis,
                    Treatment = model.Treatment,
                    Medication = model.Medication,
                    VisitDate = model.VisitDate,
                    NextAppointment = model.NextAppointment,
                    UserId = model.UserId,
                };
                await unitOfWork.PatientHestory.InsertAsync(patientHestoryToDB);
                unitOfWork.Save();

                return RedirectToAction(nameof(ShowPatientHistoryList));
            }

            List<Patient> patients = unitOfWork.Patient.GetAll()
                .Include(p => p.User).ToList();
            model.Patients = patients;

            return View(model);
        }

        public async Task<ActionResult> EditPatientHistoryRecord(Guid id)
        {
            PatientHestory? patientHestoryFromDB = await unitOfWork.PatientHestory
                .GetByIdAsync(id);

            if (patientHestoryFromDB == null)
            {
                return NotFound();
            }

            List<Patient> patients = unitOfWork.Patient.GetAll()
                .Include(p => p.User).ToList();

            PatientHestoryVM model = new PatientHestoryVM()
            {
                Id = patientHestoryFromDB.Id,
                UserId = patientHestoryFromDB.UserId,
                Diagnosis = patientHestoryFromDB.Diagnosis,
                Treatment = patientHestoryFromDB.Treatment,
                Medication = patientHestoryFromDB.Medication,
                VisitDate = patientHestoryFromDB.VisitDate,
                NextAppointment = patientHestoryFromDB.NextAppointment,
                Patients = patients
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPatientHistoryRecord(Guid id, PatientHestoryVM modelFromReq)
        {
            if (ModelState.IsValid)
            {
                PatientHestory? patientHestoryFromDB = await unitOfWork.PatientHestory
                    .GetByIdAsync(id);

                if (patientHestoryFromDB == null)
                {
                    return NotFound();
                }

                patientHestoryFromDB.UserId = modelFromReq.UserId;
                patientHestoryFromDB.Diagnosis = modelFromReq.Diagnosis;
                patientHestoryFromDB.Treatment = modelFromReq.Treatment;
                patientHestoryFromDB.Medication = modelFromReq.Medication;
                patientHestoryFromDB.VisitDate = modelFromReq.VisitDate;
                patientHestoryFromDB.NextAppointment = modelFromReq.NextAppointment;

                await unitOfWork.PatientHestory.ExecuteUpdateAsync(patientHestoryFromDB);
                unitOfWork.Save();

                return RedirectToAction(nameof(ShowPatientHistoryList));
            }
            else
            {
                List<Patient> patients = unitOfWork.Patient.GetAll()
                .Include(p => p.User).ToList();

                modelFromReq.Patients = patients;

                return View(modelFromReq);
            }
        }

        /*****************************************************/
        /* Data for Orders */
        public IActionResult ShowListOfOrderWhichDoctorSubscribed()
        {
            List<OrderVM> orderVMs = unitOfWork.Order
                .GetAll()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Appointments)
                        .ThenInclude(app => app.Doctor)
                .Include(o => o.Patient)
                    .ThenInclude(p => p.User)
                .Where(o => o.OrderItems
                    .Any(oi => oi.Appointments
                        .Any(app => app.DoctorId != null)))
                .Select(o => new OrderVM()
                {
                    Id = o.Id,
                    DateOrder = o.DateOrder,
                    PatientId = o.PatientId,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                    OrderItems = o.OrderItems.ToList(),
                    Patient = o.Patient
                }).ToList();

            return View(orderVMs);
        }

        public async Task<IActionResult> ShowDetailsOfOrderWhichDoctorSubscribed(Guid id)
        {
            Order? orderFromDB = await unitOfWork.Order.GetAll()
                .Include(o => o.Patient)
                    .ThenInclude(p => p.User).SingleOrDefaultAsync(o => o.Id == id);

            if (orderFromDB == null)
            {
                return NotFound();
            }
            OrderVM orderVM = new OrderVM()
            {
                Id = orderFromDB.Id,
                DateOrder = orderFromDB.DateOrder,
                PatientId = orderFromDB.PatientId,
                PaymentMethod = orderFromDB.PaymentMethod,
                Status = orderFromDB.Status,
                TotalPrice = orderFromDB.TotalPrice,
                Patient = orderFromDB.Patient,
                OrderItems = orderFromDB.OrderItems.ToList(),
            };

            return View();
        }
        [Authorize]
        public IActionResult CreateQueryForOrderWhichDoctorSubscriped()
        {

            List<OrderItem> orderItemsDB = unitOfWork.OrderItem
                .GetAll()
                .Include(oi => oi.Appointments)
                        .ThenInclude(app => app.Doctor)
                .Where(oi => oi.Appointments
                        .Any(app => app.DoctorId != null))
                .ToList();
            string idFromCookies = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            OrderVM orderVM = new OrderVM()
            {
                OrderItems = orderItemsDB,
                PatientId = Guid.Parse(idFromCookies)

            };

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQueryForOrderWhichDoctorSubscriped(OrderVM orderFromReq)
        {
            if (!ModelState.IsValid || !User.Identity.IsAuthenticated)
            {
                List<OrderItem> orderItemsDB = unitOfWork.OrderItem
                .GetAll()
                .Include(oi => oi.Appointments)
                    .ThenInclude(app => app.Doctor)
                .Where(oi => oi.Appointments
                    .Any(app => app.DoctorId != null))
                .ToList();

                OrderVM orderVM = new OrderVM()
                {
                    OrderItems = orderItemsDB
                };

                return View(orderVM);
            }

            /*string? idFromCookies = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var idFromCookies = User.Identity.Name;*/

            string idFromCookies = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            Order orderToDB = new Order()
            {
                DateOrder = orderFromReq.DateOrder,
                PaymentMethod = orderFromReq.PaymentMethod,
                Status = orderFromReq.Status,
                //TotalPrice = orderFromReq.TotalPrice,
                OrderItems = orderFromReq.OrderItems.ToList(),
                PatientId = Guid.Parse(idFromCookies)
            };

            await unitOfWork.Order.InsertAsync(orderToDB);
            unitOfWork.Save();

            return RedirectToAction(nameof(ShowListOfOrderWhichDoctorSubscribed));
        }

        /***************************************************/

        /**************    Druds Actions    *************/

        public IActionResult ShowDrugsList()
        {
            List<DrugsVM> drugsVM = unitOfWork.Drug
                .GetAll().Select(d => new DrugsVM()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    ActiveIngredient = d.ActiveIngredient,
                    StockQuantity = d.StockQuantity,
                    ActiveIngredientConcentration = d.ActiveIngredientConcentration,
                    HospitalId = d.HospitalId,
                    Hospital = d.Hospital,
                }).ToList();

            return View(drugsVM);
        }

        public async Task<IActionResult> ShowDetailsOfDrugs(Guid id)
        {
            Drug? drugFromDB = await unitOfWork.Drug.GetAll()
                .Include(d => d.Hospital)
                .SingleOrDefaultAsync(d => d.Id == id);
            if (drugFromDB == null)
            {
                return NotFound();
            }
            DrugsVM drugsVM = new DrugsVM()
            {
                Id = drugFromDB.Id,
                Name = drugFromDB.Name,
                HospitalId = drugFromDB.HospitalId,
                Description = drugFromDB.Description,
                Price = drugFromDB.Price,
                ActiveIngredient = drugFromDB.ActiveIngredient,
                StockQuantity = drugFromDB.StockQuantity,
                ActiveIngredientConcentration = drugFromDB.ActiveIngredientConcentration,
                Hospital = drugFromDB.Hospital,
            };
            return View(drugsVM);
        }
        public IActionResult CreateQueryForDrugs()
        {
            List<Hospital> hospitalsFromDB = unitOfWork.Hospital
                .GetAll().ToList();

            DrugsVM drugsVM = new DrugsVM()
            {
                Hospitals = hospitalsFromDB
            };
            return View(drugsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQueryForDrugs(DrugsVM drugFromReq)
        {
            if (!ModelState.IsValid)
            {
                List<Hospital> hospitalsFromDB = unitOfWork.Hospital
                                .GetAll().ToList();

                DrugsVM drugsVM = new DrugsVM()
                {
                    Hospitals = hospitalsFromDB
                };
                return View(drugFromReq);
            }
            Drug drugToDB = new Drug()
            {
                Name = drugFromReq.Name,
                Description = drugFromReq.Description,
                Price = drugFromReq.Price,
                StockQuantity = drugFromReq.StockQuantity,
                ActiveIngredient = drugFromReq.ActiveIngredient,
                ActiveIngredientConcentration = drugFromReq.ActiveIngredientConcentration,
                HospitalId = drugFromReq.HospitalId,
            };

            await unitOfWork.Drug.InsertAsync(drugToDB);
            unitOfWork.Save();

            return RedirectToAction(nameof(ShowDrugsList));
        }

        [HttpGet]
        public async Task<IActionResult> EditQuereyOfDrugs(Guid id)
        {
            Drug? drugFromDB = await unitOfWork.Drug.GetAll()
                .Include(d => d.Hospital)
                .SingleOrDefaultAsync(d => d.Id == id);
            if (drugFromDB == null)
            {
                return NotFound();
            }

            DrugsVM drugsVM = new DrugsVM()
            {
                Id = drugFromDB.Id,
                Name = drugFromDB.Name,
                HospitalId = drugFromDB.HospitalId,
                Description = drugFromDB.Description,
                Price = drugFromDB.Price,
                ActiveIngredient = drugFromDB.ActiveIngredient,
                StockQuantity = drugFromDB.StockQuantity,
                ActiveIngredientConcentration = drugFromDB.ActiveIngredientConcentration,
                Hospital = drugFromDB.Hospital,
                Hospitals = unitOfWork.Hospital.GetAll().ToList(),
            };
            
            return View(drugsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuereyOfDrugs(Guid id, DrugsVM drugsVMFromReq)
        {
            if (!ModelState.IsValid)
            {
                drugsVMFromReq.Hospitals = unitOfWork.Hospital
                        .GetAll().ToList();

                Drug? drugFromDB = await unitOfWork.Drug.GetAll()
                .Include(d => d.Hospital)
                .SingleOrDefaultAsync(d => d.Id == id);

                drugsVMFromReq.Hospital = drugFromDB.Hospital;

                return View(drugsVMFromReq);
            }

            Drug? drugToDB = await unitOfWork.Drug.GetByIdAsync(id);
            if (drugToDB == null)
            {
                return NotFound();
            }

            drugToDB.Name = drugsVMFromReq.Name;
            drugToDB.HospitalId = drugsVMFromReq.HospitalId;
            drugToDB.Description = drugsVMFromReq.Description;
            drugToDB.Price = drugsVMFromReq.Price;
            drugToDB.ActiveIngredient = drugsVMFromReq.ActiveIngredient;
            drugToDB.StockQuantity = drugsVMFromReq.StockQuantity;
            drugToDB.ActiveIngredientConcentration = drugsVMFromReq.ActiveIngredientConcentration;

            await unitOfWork.Drug.ExecuteUpdateAsync(drugToDB);
            unitOfWork.Save();

            return RedirectToAction(nameof(ShowDrugsList));
        }

        /***************************************************/

        /**************    Labs Actions    *************/
        
        public IActionResult ShowLabsList()
        {
            List<LabsVM> labsVM = unitOfWork.Lab
                .GetAll().Include(l => l.Hospital).Select(l => new LabsVM()
                {
                    Id = l.Id,
                    Name = l.Name,
                    HospitalId = l.HospitalId,
                    Price = l.Price,
                    Hospital = l.Hospital,
                    //TestLab = l.TestLab.ToList(),
                }).ToList();

            return View(labsVM);
        }

        public async Task<IActionResult> ShowDetailsOfLabs(Guid id)
        {
            Lab? labFromDB = await unitOfWork.Lab.GetAll()
                .Include(l => l.Hospital)
                .SingleOrDefaultAsync(l => l.Id == id);

            if (labFromDB == null)
            {
                return NotFound();
            }
            LabsVM labsVM = new LabsVM()
            {
                Id = labFromDB.Id,
                Name = labFromDB.Name,
                HospitalId = labFromDB.HospitalId,
                Price = labFromDB.Price,
                //TestLab = labFromDB.TestLab.ToList(),2
                Hospital = labFromDB.Hospital
            };

            return View(labsVM);
        }

        public IActionResult CreateQueryForLabs()
        {
            List<Hospital> hospitalsFromDB = unitOfWork.Hospital
                .GetAll().ToList();
            List<TestLab> testlabFromDB = unitOfWork.TestLab
                .GetAll().ToList();
            LabsVM labsVM = new LabsVM()
            {
                //TestLab = testlabFromDB.ToList(),
                Hospitals = hospitalsFromDB.ToList(),
            };
            return View(labsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQueryForLabs(LabsVM labsVMFromReq)
        {
            if (!ModelState.IsValid)
            {
                List<Hospital> hospitalsFromDB = unitOfWork.Hospital
                .GetAll().ToList();
                List<TestLab> testlabFromDB = unitOfWork.TestLab
                    .GetAll().ToList();

                LabsVM labsVM = new LabsVM()
                {
                    //TestLab = testlabFromDB.ToList(),
                    Hospitals = hospitalsFromDB.ToList(),
                };
                return View(labsVMFromReq);
            }

            Lab labToDB = new Lab()
            {
                Name = labsVMFromReq.Name,
                Price = labsVMFromReq.Price,
                //TestLab = labsVMFromReq.TestLab,
                HospitalId = labsVMFromReq.HospitalId,
            };

            await unitOfWork.Lab.InsertAsync(labToDB);
            unitOfWork.Save();

            return RedirectToAction(nameof(ShowLabsList));
        }

        [HttpGet]
        public async Task<IActionResult> EditQuereyOfLabs(Guid id)
        {
            Lab? labFromDB = await unitOfWork.Lab.GetAll()
                .Include(l => l.Hospital)
                .SingleOrDefaultAsync(l => l.Id == id);
            if (labFromDB == null)
            {
                return NotFound();
            }

            LabsVM labVM = new ()
            {
                Id  = labFromDB.Id,
                Name = labFromDB.Name,
                HospitalId = labFromDB.HospitalId,
                Price = labFromDB.Price,
                Hospitals = unitOfWork.Hospital.GetAll().ToList(),
                Hospital = labFromDB.Hospital,
            };

            return View(labVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuereyOfLabs(Guid id , LabsVM labVMFromReq)
        {       
            if (!ModelState.IsValid)
            {
                labVMFromReq.Hospitals = unitOfWork.Hospital
                        .GetAll().ToList();

                return View(labVMFromReq); 
            }

            Lab? labToDB = await unitOfWork.Lab.GetByIdAsync(id);
            if (labToDB == null)
            {
                return NotFound();
            }

            labToDB.Name = labVMFromReq.Name;
            labToDB.HospitalId = labVMFromReq.HospitalId;
            labToDB.Price = labVMFromReq.Price;

            await unitOfWork.Lab.ExecuteUpdateAsync(labToDB);
            unitOfWork.Save();

            return RedirectToAction(nameof(ShowLabsList));
        }
    }
}

