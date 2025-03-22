using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class Drug:BaseEntity
    {
        [Key]
        public int DrugId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required, StringLength(100)]
        public string ActiveIngredient { get; set; }

        [Required]
        public double ActiveIngredientConcentration { get; set; }

        [ForeignKey("Hospital")]
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }


    }
}
