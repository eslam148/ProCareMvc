namespace ProCareMvc.presentation.Models
{
    public class BookAppointmentVM
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime StartTime { get; set; }
        public double Amount { get; set; }
    }
}
