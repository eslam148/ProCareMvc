using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Utility
{
   public enum Gender
    {
        Male = 0,
        Female = 1,
    }  
    public enum OrderItemType
    {
        General,
        Drugs ,
        Appointments,
        Labs,
    }

    public enum Role
    {
        Administrator,
        Doctor,
        Patient
    }
}
