using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProCareMvc.business.Interface;
using ProCareMvc.business.InterfaceReposatory;
using ProCareMvc.business.Repository;
using ProCareMvc.Database;

namespace ProCareMvc.business
{
    public class UnitOfWork(AppDbContext appContext) : IUnitOfWork
    {
        private AppDbContext context = appContext;

        public IOrderRepository Order { get; } = new OrderRepository(appContext);

        public IOrderItemRepository OrderItem { get; } = new OrderItemRepository(appContext);

        public IDrugRepository Drug { get; } = new DrugRepository(appContext);

        public IHospitalRepository Hospital { get; } = new HospitalRepository(appContext);

        public ITakeDrugRepository takeDrug { get; } = new TakeDrugRepository(appContext);

 
        public IAppointmentRepository Appointment { get; } = new AppointmentRepository(appContext);

        public IDepartmentRepository Department { get; } = new DepartmentRepository(appContext);

        public IDoctorRepository Doctor { get; } = new DoctorRepository(appContext);

        public IPatientRepository Patient { get; } = new PatientRepository(appContext);

        public ILabRepository Lab { get; } = new LabRepository(appContext);

        public ITestLabRepository TestLab { get; } = new TestLabRepository(appContext);

        public void Dispose()
        {
            context.Dispose();
        }
        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
