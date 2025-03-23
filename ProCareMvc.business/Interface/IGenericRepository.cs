using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.business.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //get all with pagination
        Task<IEnumerable<TEntity>> GetAllPaginationAsync(int pageNumber, int pageSize);
        //ExecuteUpdateAsync
        Task ExecuteUpdateAsync(TEntity entity);
        // ExecuteDeleteAsync
        Task ExecuteDeleteAsync(Guid id);

    }
}
