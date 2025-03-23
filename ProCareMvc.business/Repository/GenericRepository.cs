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

        public async Task<TEntity?> GetByIdAsync(Guid id)
        
        {
         
           
                TEntity? element = await _appDbContext.Set<TEntity>().FindAsync(id);
                return element;
            
  

        }


        public  IQueryable<TEntity> GetAll()

        {
        return   _appDbContext.Set<TEntity>();


        }
    }
}
