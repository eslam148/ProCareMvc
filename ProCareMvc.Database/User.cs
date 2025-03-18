using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProCareMvc.Database
{
    public class User  : IdentityUser<Guid>
    {
        
    }
}
