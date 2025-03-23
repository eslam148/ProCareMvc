using Microsoft.EntityFrameworkCore;
using ProCareMvc.business.Interface;
using ProCareMvc.Database;

namespace ProCareMvc.business.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task ExecuteDeleteAsync(Guid id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                
            }

        }
        //ExecuteUpdateAsync
        public async Task ExecuteUpdateAsync(TEntity entity)
        {

            dbSet.Update(entity);
            await _appDbContext.SaveChangesAsync();

        }

       

        public Task<IQueryable<TEntity>> GetAllPaginationAsync(int pageNumber, int pageSize)
        {
            return (Task<IQueryable<TEntity>>)dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

    }
}
