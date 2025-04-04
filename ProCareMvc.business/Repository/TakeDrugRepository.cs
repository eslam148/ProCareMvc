﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProCareMvc.business.Interface;
using ProCareMvc.Database;
using ProCareMvc.Database.Entity;

namespace ProCareMvc.business.Repository
{
    public class TakeDrugRepository : GenericRepository<TakeDrug>, ITakeDrugRepository
    {
        public TakeDrugRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
