using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Price { get; set; }
    }
}
