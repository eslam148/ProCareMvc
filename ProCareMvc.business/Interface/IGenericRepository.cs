﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.business.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<TEntity?> GetByIdAsync(Guid id);
        public IQueryable<TEntity> GetAll();


    }
}
