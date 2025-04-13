namespace ProCareMvc.presentation.Models
{
    public class AppointmentVM
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public Guid PatientId { get; set; }
    }
}
