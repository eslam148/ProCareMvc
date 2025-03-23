using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class DrugOrderItem:OrderItem
    {
        [ForeignKey(nameof(Drug))]
        public Guid DrugId { get; set; }
        public Drug Drug { get; set; }
    }
}
