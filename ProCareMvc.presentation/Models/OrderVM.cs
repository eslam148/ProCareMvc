using ProCareMvc.Database.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProCareMvc.presentation.Models
{
    public class OrderVM
    {
        public Guid Id { get; set; }

        [ForeignKey("Patient")]
        public Guid? PatientId { get; set; }

        public string PaymentMethod { get; set; }

        public decimal TotalPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOrder { get; set; } = DateTime.Now;
        public string Status { get; set; }

        public Patient Patient { get; set; }
        public List<OrderItem> OrderItems { get; set; } //show list of order which subscribe with doc ID
        public List<Appointment> Appointments { get; set; }
    }
}
