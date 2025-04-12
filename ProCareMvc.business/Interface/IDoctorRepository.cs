using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProCareMvc.Database.Entity;
using Microsoft.EntityFrameworkCore;




namespace ProCareMvc.business.Interface
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        


    }
}
