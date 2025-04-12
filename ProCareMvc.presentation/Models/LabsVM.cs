using ProCareMvc.Database.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProCareMvc.presentation.Models
{
    public class LabsVM
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public List<TestLab> TestLab { get; set; }

        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public List<Hospital> Hospitals { get; set; }

    }
}
