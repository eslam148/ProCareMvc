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
                await _appDbContext.SaveChangesAsync();
            }

        }
        //ExecuteUpdateAsync
        public async Task ExecuteUpdateAsync(TEntity entity)
        {

            dbSet.Update(entity);
            await _appDbContext.SaveChangesAsync();

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
            await _appDbContext.AddAsync(entity);
        }
        public async Task InsertAllAsync(ICollection<TEntity> entities)
        public async Task<TEntity?> GetByIdAsync(Guid id)
        
        {
         
           
                TEntity? element = await _appDbContext.Set<TEntity>().FindAsync(id);
                return element;
            
  

        }


        public  IQueryable<TEntity> GetAll()

        {
            await _appDbContext.AddRangeAsync(entities);
        return   _appDbContext.Set<TEntity>();


        }


    }
}
