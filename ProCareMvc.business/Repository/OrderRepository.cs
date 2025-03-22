using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProCareMvc.business.InterfaceReposatory;
using ProCareMvc.Database;
using ProCareMvc.Database.Entity;

namespace ProCareMvc.business.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
