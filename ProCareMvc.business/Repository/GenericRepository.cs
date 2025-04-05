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
            

        }

        public async Task<IEnumerable<TEntity>> GetAllPaginationAsync(int pageNumber, int pageSize)
        {
            return await dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task InsertAsync(TEntity entity)
        {
            await _appDbContext.Set<TEntity>().AddAsync(entity);
        }
        public async Task InsertAllAsync(ICollection<TEntity> entities)
        {
            await _appDbContext.Set<TEntity>().AddRangeAsync(entities);

        }
        public async Task<TEntity?> GetByIdAsync(Guid id)     
        {
                TEntity? element = await _appDbContext.Set<TEntity>().FindAsync(id);
                return element;
        }
        public  IQueryable<TEntity> GetAll()
        {
            return _appDbContext.Set<TEntity>();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }


    }
}
