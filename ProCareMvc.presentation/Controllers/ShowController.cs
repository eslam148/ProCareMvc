using Microsoft.AspNetCore.Mvc;//
using Microsoft.EntityFrameworkCore;//
using ProCareMvc.business;//
using ProCareMvc.Database.Entity;//
using ProCareMvc.presentation.Models;//
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProCareMvc.presentation.Controllers
{
    public class ShowController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ShowController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult ShowPatientHistoryList()
        {
            List<PatientHestoryVM> hestoryVMs = unitOfWork.PatientHestory
                .GetAll().Select(ph => new PatientHestoryVM()
                {
                    Id = ph.Id,
                    Diagnosis = ph.Diagnosis,
                    Treatment = ph.Treatment,
                    Medication = ph.Medication,
                    VisitDate = ph.VisitDate,
                    NextAppointment = ph.NextAppointment,
                    UserId = ph.UserId,
                }).ToList();
            return View(hestoryVMs);
        }

        // GET: ShowController/Details/5
        public async Task<ActionResult> ShowDetailsOfPatientHistory(Guid id)
        {
            PatientHestory patnthstryFrmDB = await unitOfWork.PatientHestory.GetByIdAsync(id);
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
            };
            return View(patenthstryVM);
        }

        [HttpGet]
        public IActionResult CreatePatientHistoryRecord()
        {
            var usersList = unitOfWork.Patient.GetAll()
                .Include(p => p.User)
                .Select(p => new SelectListItem
                {
                    Value = p.UserId.ToString(),
                    Text = p.User.FirstName + " " + p.User.LastName,
                }).ToList();

            PatientHestoryVM patientHestory = new PatientHestoryVM()
            {
                UsersList = usersList
            };
            return View(patientHestory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatientHistoryRecord(PatientHestoryVM model)
        {
            if (ModelState.IsValid && model.NextAppointment > model.VisitDate)
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

            var usersList = unitOfWork.Patient.GetAll()
                .Include(p => p.User)
                .Select(p => new SelectListItem
                {
                    Value = p.UserId.ToString(),
                    Text = p.User.FirstName + " " + p.User.LastName,
                }).ToList();

            PatientHestoryVM patientHestory = new PatientHestoryVM()
            {
                UsersList = usersList
            };
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

            var usersList = unitOfWork.Patient.GetAll()
                .Include(p => p.User)
                .Select(p => new SelectListItem
                {
                    Value = p.UserId.ToString(),
                    Text = p.User.FirstName + " " + p.User.LastName
                }).ToList();

            PatientHestoryVM model = new PatientHestoryVM()
            {
                Id = patientHestoryFromDB.Id,
                UserId = patientHestoryFromDB.UserId,
                Diagnosis = patientHestoryFromDB.Diagnosis,
                Treatment = patientHestoryFromDB.Treatment,
                Medication = patientHestoryFromDB.Medication,
                VisitDate = patientHestoryFromDB.VisitDate,
                NextAppointment = patientHestoryFromDB.NextAppointment,
                UsersList = usersList
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPatientHistoryRecord(Guid id, PatientHestoryVM modelFromReq)
        {
            if (ModelState.IsValid && modelFromReq.NextAppointment > modelFromReq.VisitDate)
            {
                PatientHestory? patientHestoryFromDB = await unitOfWork.PatientHestory
                    .GetByIdAsync(id);

                if (patientHestoryFromDB == null)
                {
                    return NotFound();
                }

                patientHestoryFromDB.Id = modelFromReq.Id;
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
                var usersList = unitOfWork.Patient.GetAll()
                .Include(p => p.User)
                .Select(p => new SelectListItem
                {
                    Value = p.UserId.ToString(),
                    Text = p.User.FirstName + " " + p.User.LastName
                }).ToList();

                modelFromReq.UsersList = usersList;

                return View(modelFromReq);
            }
        }
    }
}
