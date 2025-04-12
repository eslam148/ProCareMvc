using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProCareMvc.business;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;

namespace ProCareMvc.presentation.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        // GET: DepartmentController
        public ActionResult Index()
        {
            List<DepartmentVM> departmentList = unitOfWork.Department
                .GetAll()
                .Select(d => new DepartmentVM
                {
                    Id = d.Id,
                    Name = d.Name,
                    ManagerId = d.ManagerId,
                    Hospitals = unitOfWork.Hospital.GetAll().ToList()
                    /*
                     Hospitals = unitOfWork.Hospital
                    .GetAll()
                    .Select( h => new Hospital()
                    {
                        Id = h.Id,
                        Name = h.Name
                    })
                    .ToList()
                     */
                }).ToList();
            return View(departmentList);
        }
       
        // GET: DepartmentController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            Department department = await unitOfWork.Department.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            Doctor? doctor = await unitOfWork.Doctor.GetByIdAsync(department.ManagerId);
            Hospital? hospital = await unitOfWork.Hospital.GetByIdAsync(department.HospitalId);

            var departmentVM = new DepartmentVM
            {
                Name = department.Name,
                ManagerId = department.ManagerId,
                HospitalId = department.HospitalId,
                DoctorObj = doctor,
                Hospital = hospital
            };
            return View(departmentVM);
        }

        // GET: DepartmentController/Create
        public IActionResult Create()
        {

            DepartmentVM model = new DepartmentVM
            {
                Hospitals = unitOfWork.Hospital.GetAll().ToList(),
                Doctors = unitOfWork.Doctor.GetAll().ToList(),
                /*ManagerId = Guid.Parse(guidString)*/
            };
            return View(model);
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DepartmentVM deptFromReq)
        {
            if (ModelState.IsValid)
            {
               
                Department DeptToDB = new Department
                {
                    Name = deptFromReq.Name,
                    HospitalId = deptFromReq.HospitalId,
                    ManagerId = deptFromReq.ManagerId,
                };
                await unitOfWork.Department.InsertAsync(DeptToDB);
                unitOfWork.Save();
                return RedirectToAction("Index");
                                
            }
            deptFromReq.Hospitals = unitOfWork.Hospital.GetAll().ToList();
            deptFromReq.Doctors = unitOfWork.Doctor.GetAll().ToList();
            return View(deptFromReq);
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            Department deptFromDB = await unitOfWork.Department.GetByIdAsync(id);
            if (deptFromDB == null)
            {
                return NotFound();
            }
            var departmentVM = new DepartmentVM()
            {
                Id = deptFromDB.Id,  
                Name = deptFromDB.Name,
                ManagerId = deptFromDB.ManagerId,
                HospitalId = deptFromDB.HospitalId,
                Hospitals = unitOfWork.Hospital.GetAll().ToList(),
                Doctors = unitOfWork.Doctor.GetAll().ToList()
            };
            return View(departmentVM);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, DepartmentVM deptFromReq)
        {
            if (id != deptFromReq.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                deptFromReq.Hospitals = unitOfWork.Hospital.GetAll().ToList();
                deptFromReq.Doctors = unitOfWork.Doctor.GetAll().ToList();
                return View(deptFromReq);
            }
            
                Department deptFromDB = await unitOfWork.Department.GetByIdAsync(id);
                if (deptFromDB == null)
                {
                    return NotFound();
                }
                deptFromDB.Name = deptFromReq.Name;
                deptFromDB.ManagerId = deptFromReq.ManagerId;
                deptFromDB.HospitalId = deptFromReq.HospitalId;

                await unitOfWork.Department.ExecuteUpdateAsync(deptFromDB);
            try
            {
                unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating: " + ex.Message);
                deptFromReq.Hospitals = unitOfWork.Hospital.GetAll().ToList();
                deptFromReq.Doctors = unitOfWork.Doctor.GetAll().ToList();
                return View(deptFromReq);
            }
        }

        // GET: DepartmentController/Delete/5
        /*public Task<ActionResult> Delete(int id)
        {
            return View();
        }

        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
