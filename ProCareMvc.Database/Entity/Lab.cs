using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class Lab:BaseEntity
    {
        public decimal Price { get; set; }

        public ICollection<TestLab> TestLab { get; set; }

        [ForeignKey(nameof(Hospital))]
        public Guid HospitalId { get; set; }
        Hospital Hospital { get; set; }
    }
}
