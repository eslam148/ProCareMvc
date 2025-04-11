using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProCareMvc.Database.Utility;

namespace ProCareMvc.Database.Entity
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Patient? Patients { get; set; }
        public Doctor? Doctor { get; set; }
        public string ImageProfileUrl { get; set; } = "doc-5.jpg";

        public ICollection<PatientHestory>? PatientHestories { get; set; } = new List<PatientHestory>();
    }
}
