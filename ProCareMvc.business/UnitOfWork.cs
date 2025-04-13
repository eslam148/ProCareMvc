using ProCareMvc.business.Interface;
using ProCareMvc.business.InterfaceReposatory;
using ProCareMvc.business.Repository;
using ProCareMvc.business;
using ProCareMvc.Database;

public class UnitOfWork(AppDbContext appContext) : IUnitOfWork
{
    private readonly AppDbContext _context = appContext;

    //public IOrderRepository Order { get; }
    //public IOrderItemRepository OrderItem { get; }
    //public IDrugRepository Drug { get; }
    //public IHospitalRepository Hospital { get; }
    //public ITakeDrugRepository TakeDrug { get; }
    //public IAppointmentRepository Appointment { get; }
    //public IDepartmentRepository Department { get; }
    //public IDoctorRepository Doctor { get; }
    //public IPatientRepository Patient { get; }
    //public ILabRepository Lab { get; }
    //public ITestLabRepository TestLab { get; }
    //public IUserRepository User { get; }

    
        public IOrderRepository Order { get; } = new OrderRepository(appContext);

        public IOrderItemRepository OrderItem { get; } = new OrderItemRepository(appContext);

        public IDrugRepository Drug { get; } = new DrugRepository(appContext);

        public IHospitalRepository Hospital { get; } = new HospitalRepository(appContext);

        public ITakeDrugRepository TakeDrug { get; } = new TakeDrugRepository(appContext);

 
        public IAppointmentRepository Appointment { get; } = new AppointmentRepository(appContext);

        public IDepartmentRepository Department { get; } = new DepartmentRepository(appContext);

        public IDoctorRepository Doctor { get; } = new DoctorRepository(appContext);

        public IPatientRepository Patient { get; } = new PatientRepository(appContext);

        public ILabRepository Lab { get; } = new LabRepository(appContext);

        public ITestLabRepository TestLab { get; } = new TestLabRepository(appContext);
        public IPatientHestoryRepository PatientHestory { get; } = new PatientHestoryRepository(appContext);

        public IUserRepository User { get; } = new UserRepository(appContext);

         

    public void Dispose()
    {
        _context.Dispose();
    }

    public int Save()
    {
        return _context.SaveChanges();
    }
}
