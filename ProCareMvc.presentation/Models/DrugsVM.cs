using ProCareMvc.Database.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class DrugsVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string ActiveIngredient { get; set; }

        public double ActiveIngredientConcentration { get; set; }

        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public List<Hospital> Hospitals { get; set; }


    }
}
