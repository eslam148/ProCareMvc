using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProCareMvc.business;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;

namespace ProCareMvc.presentation.Controllers
{
    public class HospitalController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HospitalController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: HospitalController
        public ActionResult Index()
        {
            var hospitalList = unitOfWork.Hospital.GetAll()
                .Select(h => new HospitalVM
                {
                    ID = h.Id,
                    Name = h.Name,
                    Address = h.Address,
                    PhoneNumber = h.PhoneNumber,
                    Email = h.Email
                }).ToList();
            return View(hospitalList);
        }

        // GET: HospitalController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            Hospital hospital = await unitOfWork.Hospital.GetByIdAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            var hospitalVM = new HospitalVM
            {
                Name = hospital.Name,
                Address = hospital.Address,
                PhoneNumber = hospital.PhoneNumber,
                Email = hospital.Email
            };
            return View(hospitalVM);
        }

        // GET: HospitalController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: HospitalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HospitalVM hosptalReq)
        {
            if (ModelState.IsValid)
            {
                Hospital hospitalToDB = new Hospital();
                hospitalToDB.Address = hosptalReq.Address;
                hospitalToDB.Name = hosptalReq.Name;
                hospitalToDB.Email = hosptalReq.Email;
                hospitalToDB.PhoneNumber = hosptalReq.PhoneNumber;
                hospitalToDB.Departments = hosptalReq.Departments;

                await unitOfWork.Hospital.InsertAsync(hospitalToDB);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(hosptalReq);

        }

        // GET: HospitalController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            Hospital hospitalFromDb = await unitOfWork.Hospital.GetByIdAsync(id);
            if (hospitalFromDb == null)
            {
                return NotFound();
            }
            var hospitalVM = new HospitalVM
            {
                Name = hospitalFromDb.Name,
                Address = hospitalFromDb.Address,
                PhoneNumber = hospitalFromDb.PhoneNumber,
                Email = hospitalFromDb.Email
            };
            return View(hospitalVM);
        }

        // POST: HospitalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, HospitalVM hospitalVM)
        {
            if (!ModelState.IsValid)
            {
                return View(hospitalVM);
            }

            Hospital hospital = await unitOfWork.Hospital.GetByIdAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }

            hospital.Name = hospitalVM.Name;
            hospital.Address = hospitalVM.Address;
            hospital.PhoneNumber = hospitalVM.PhoneNumber;
            hospital.Email = hospitalVM.Email;

            await unitOfWork.Hospital.ExecuteUpdateAsync(hospital);
            unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // GET: HospitalController/Delete/5


        // POST: HospitalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            var hospitalFromDb = await unitOfWork.Hospital.GetByIdAsync(id);
            if (hospitalFromDb == null)
            {
                return NotFound();
            }

            await unitOfWork.Hospital.ExecuteDeleteAsync(id);

            // Ensure changes are saved after deletion
             unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

    }
}
