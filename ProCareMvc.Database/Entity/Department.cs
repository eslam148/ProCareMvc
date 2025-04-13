using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class Department:BaseEntity
    {
        
        [ForeignKey(nameof(Manager))]
        public Guid ManagerId { get; set; }
        public Doctor Manager { get; set; }     
        
        [ForeignKey(nameof(Hospital))]
        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }

     }
}
