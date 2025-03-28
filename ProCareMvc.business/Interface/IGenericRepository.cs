using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.business.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // ExecuteDeleteAsync

        Task ExecuteDeleteAsync(Guid id);
        //ExecuteUpdateAsync
        Task ExecuteUpdateAsync(TEntity entity);
        //get all with pagination
        Task<IEnumerable<TEntity>> GetAllPaginationAsync(int pageNumber, int pageSize);

        Task InsertAsync(TEntity entity);

        Task InsertAllAsync(ICollection<TEntity> entities);
        Task<TEntity?> GetByIdAsync(Guid id);
        IQueryable<TEntity> GetAll();


    }
}
