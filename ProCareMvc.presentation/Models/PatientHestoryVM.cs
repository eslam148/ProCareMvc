using Microsoft.AspNetCore.Mvc.Rendering;
using ProCareMvc.Database.Entity;
using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class PatientHestoryVM
    {
        public Guid Id { get; set; }
        public string Treatment { get; set; }

        public string Diagnosis { get; set; }

        public string Medication { get; set; }

        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime NextAppointment { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; }
        public List<SelectListItem> UsersList { get; set; }
    }
}
