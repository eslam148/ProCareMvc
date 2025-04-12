using ProCareMvc.Database.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProCareMvc.presentation.Models
{
    public class DepartmentVM
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid ManagerId { get; set; }
        //public string ManegerName { get; set; }   

        public Guid HospitalId { get; set; }
        public List<Hospital> Hospitals { get; set; }
        public List<Doctor> Doctors { get; set; }

        public Doctor DoctorObj { get; set; } 
        public Hospital Hospital { get; set; }
        

    }
}
