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
    public class UnitOfWork:IUnitOfWork
    {
        private AppDbContext context;

        public IOrderRepository OrderRepository {get;}

        public IOrderItemRepository OrderItemRepository {get;}

        public IDrugRepository drugRepository {get;}

        public IHospitalRepository HospitalRepository {get;}

        public ITakeDrugRepository takeDrugRepository {get;}

        public UnitOfWork(AppDbContext appContext)
        {
            context = appContext;
            OrderRepository = new OrderRepository(appContext);
            OrderItemRepository = new OrderItemRepository(appContext);
            drugRepository = new DrugRepository(appContext);
            HospitalRepository = new HospitalRepository(appContext);
            takeDrugRepository = new TakeDrugRepository(appContext);
        }

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
