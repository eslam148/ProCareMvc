using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class AppointmentOrderItem:OrderItem
    {
        [ForeignKey(nameof(Appointment))]
        public Guid AppointmetnId { get; set; }
        public Appointment Appointment { get; set; }

    }
}
