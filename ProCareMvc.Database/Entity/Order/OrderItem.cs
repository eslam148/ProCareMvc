using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProCareMvc.Database.Entity
{
    public class OrderItem
    {


        public ICollection<Drug> Drugs { get; set; }
        public ICollection<Lab> Labs { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

        public Order Order { get; set; }
    }
}
