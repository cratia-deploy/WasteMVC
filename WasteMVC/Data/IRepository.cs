using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WasteMVC.Models;

namespace WasteMVC.Data
{
    public interface IRepository<TEntity> 
        where TEntity : EntityBase
    {
        bool Add(List<TEntity> _objects);
        bool Add(TEntity _object);
        bool Delete(Expression<Func<TEntity, bool>> filter);
        bool Delete(int _id);
        TEntity Find(Expression<Func<TEntity, bool>> filter);
        TEntity Find(int _id);
        TEntity Firts();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
        TEntity Last();
        bool Update(TEntity _objectupdate);
    }
}