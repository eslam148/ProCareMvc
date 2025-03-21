using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProCareMvc.Database.Entity
{
    public class OrderItem
    {
        [ForeignKey("Order")] 
        public int OrderId { get; set; }

        public virtual List<Drug> DrugList { get; set; } = new List<Drug>();
        public virtual List<Lab> LabList { get; set; } = new List<Lab>();
        public virtual List<Appointment> AppointmentList { get; set; } = new List<Appointment>();

        public virtual Order Order { get; set; }
    }
}
