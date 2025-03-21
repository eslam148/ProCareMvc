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
        public int TransactionId { get; set; } //Number of order

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public string PaymentMethod { get; set; }
        
        public decimal TotalPrice { get; set; }
        public DateTime DateOrder {  get; set; }
        public string Status {  get; set; }

        public List<OrderItem> OrderItemList { get; set; }
        public virtual Patient Patient { get; set; }

    }
}
