using ProCareMvc.Database.Utility;
using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class PatientRegisterVM: RegisterVM
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }

    }
}
