using Microsoft.EntityFrameworkCore;
using ProCareMvc.business.Interface;
using ProCareMvc.Database;
using ProCareMvc.Database.Entity;
using System;
using System.Threading.Tasks;

namespace ProCareMvc.business.Repository
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

       
    }
}
