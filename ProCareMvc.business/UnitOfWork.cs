using ProCareMvc.business.Interface;
using ProCareMvc.business.InterfaceReposatory;
using ProCareMvc.business.Repository;
using ProCareMvc.business;
using ProCareMvc.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IOrderRepository Order { get; }
    public IOrderItemRepository OrderItem { get; }
    public IDrugRepository Drug { get; }
    public IHospitalRepository Hospital { get; }
    public ITakeDrugRepository TakeDrug { get; }
    public IAppointmentRepository Appointment { get; }
    public IDepartmentRepository Department { get; }
    public IDoctorRepository Doctor { get; }
    public IPatientRepository Patient { get; }
    public ILabRepository Lab { get; }
    public ITestLabRepository TestLab { get; }
    public IUserRepository User { get; }

    public UnitOfWork(AppDbContext appContext)
    {
        _context = appContext;

        Order = new OrderRepository(appContext);
        OrderItem = new OrderItemRepository(appContext);
        Drug = new DrugRepository(appContext);
        Hospital = new HospitalRepository(appContext);
        TakeDrug = new TakeDrugRepository(appContext);
        Appointment = new AppointmentRepository(appContext);
        Department = new DepartmentRepository(appContext);
        Doctor = new DoctorRepository(appContext);
        Patient = new PatientRepository(appContext);
        Lab = new LabRepository(appContext);
        TestLab = new TestLabRepository(appContext);
        User = new UserRepository(appContext);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public int Save()
    {
        return _context.SaveChanges();
    }
}
