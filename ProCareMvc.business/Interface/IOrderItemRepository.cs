using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProCareMvc.business.Interface;
using ProCareMvc.Database.Entity;

namespace ProCareMvc.business.InterfaceReposatory
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
    }
}
