using ProCareMvc.Database.Entity;

namespace ProCareMvc.presentation.Models
{
    public class PatientHestoryVM
    {
        public Guid Id { get; set; }
        public string Treatment { get; set; }

        public string Diagnosis { get; set; }

        public string Medication { get; set; }

        public DateTime VisitDate { get; set; }

        public DateTime NextAppointment { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; }
    }
}
