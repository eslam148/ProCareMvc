using ProCareMvc.Database.Entity;
using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class HospitalVM
    {
        public Guid? ID { get; set; }
        [Required]
        public string Name {  get; set; }
        public string Address { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public List<Department>? Departments { get; set; }
    }
}
