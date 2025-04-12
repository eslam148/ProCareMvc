using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class Order
    {

        public Guid Id { get; set; }

        [ForeignKey("Patient")]
        public Guid PatientId { get; set; }

        public string PaymentMethod { get; set; }
        
        public int TotalPrice { get; set; }
        public DateTime DateOrder {  get; set; }
        public string Status {  get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } //show list of order which subscribe with doc ID
        public Patient Patient { get; set; }


    }
}
