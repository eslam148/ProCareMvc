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
            OrderRepository = new OrderRepository(context);
            OrderItemRepository = new OrderItemRepository(context);
            drugRepository = new DrugRepository(context);
            HospitalRepository = new HospitalRepository(context);
            takeDrugRepository = new TakeDrugRepository(context);
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
