using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class TakeDrug
    {
        [Key]
        public int  Id { get; set; }

        //[Required]
        //public DateTime TakenAt { get; set; }

        //[Required]
        //public int Quantity { get; set; }

        [ForeignKey("Drug")]
        public Guid DrugId { get; set; }
        public Drug Drug { get; set; }

        [ForeignKey("PatientHestory")]
        public Guid PatientHistoryId { get; set; }
        public PatientHestory PatientHistory { get; set; }
    }
       
}
