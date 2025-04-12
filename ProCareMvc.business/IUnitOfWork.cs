using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProCareMvc.business.Interface;
using ProCareMvc.business.InterfaceReposatory;

namespace ProCareMvc.business
{
    
    public interface IUnitOfWork: IDisposable
    {
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }
        IDrugRepository Drug { get; }
        IHospitalRepository Hospital { get; }
        ITakeDrugRepository TakeDrug { get; }
        IAppointmentRepository  Appointment { get; }
        IDepartmentRepository Department { get; }
        IDoctorRepository Doctor { get; }
        IPatientRepository Patient { get; }
        ILabRepository Lab { get; }
        ITestLabRepository TestLab { get; }
        IPatientHestoryRepository PatientHestory { get; }
        IUserRepository User { get; }
       

        int Save();
    }
}
