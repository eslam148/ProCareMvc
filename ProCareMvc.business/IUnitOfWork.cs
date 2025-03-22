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
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IDrugRepository drugRepository { get; }
        IHospitalRepository HospitalRepository { get; }
        ITakeDrugRepository takeDrugRepository { get; }
        int Save();
    }
}
