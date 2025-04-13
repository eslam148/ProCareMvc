namespace ProCareMvc.presentation.Models
{
    public class ShowDoctorInHomePageVM
    {

        public Guid Id { get; set; } // المعرف الفريد للطبيب
        public string FullName { get; set; } // مزيج من الاسم الأول والأخير من جدول AspNetUsers
        public string DepartmentName { get; set; } // اسم القسم من جدول Departments
        public int YearsOfExperience { get; set; } // سنوات الخبرة من جدول Doctors
        public string PhoneNumber { get; set; } // رقم الهاتف من جدول AspNetUsers
        public bool IsDeleted { get; set; } // لتصفية الأطباء المحذوفين
        public string ImageUrl { get; set; }

        public List<AppointmentVM> AvailableAppointments { get; set; }

    }
}
