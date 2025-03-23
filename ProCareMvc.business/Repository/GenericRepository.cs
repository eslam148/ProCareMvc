using ProCareMvc.business.Interface;
using ProCareMvc.Database;

namespace ProCareMvc.business.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _appDbContext;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public TEntity GetById(Guid id)
        {
            return _appDbContext.Set<TEntity>().Find(id);
        }


        public async Task InsertAsync(TEntity entity)
        {
            await _appDbContext.AddAsync(entity);
        }
        public async Task InsertAllAsync(ICollection<TEntity> entities)
        {
            await _appDbContext.AddRangeAsync(entities);
        }


    }
}
