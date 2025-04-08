using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class Doctor
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(Department))]
        public Guid DepartmentId { get; set; }

        [ForeignKey(nameof(Hospital))]
        public Guid HospitalId { get; set; }
    
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public int YearsOfExperience { get; set; }

        //Navigation Proparity
        public User User { get; set; }

        public Department Department { get; set; }
        public Hospital Hospital { get; set; }
    }
}
