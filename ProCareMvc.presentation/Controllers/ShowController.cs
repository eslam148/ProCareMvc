using Microsoft.AspNetCore.Mvc;
using ProCareMvc.business;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;

namespace ProCareMvc.presentation.Controllers
{
    public class ShowController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ShowController(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult IndexPH()
        {
            List<PatientHestoryVM> hestoryVMs = unitOfWork.PatientHestory
                .GetAll().Select( ph => new PatientHestoryVM() {
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
        public async Task<ActionResult> DetailsPH(Guid id)
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
        


    }
}
