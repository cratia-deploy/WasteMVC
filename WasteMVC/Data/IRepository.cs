using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
        TEntity Last();
        bool Update(TEntity _objectupdate);
        bool Any(Expression<Func<TEntity, bool>> filter);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);
    }
}
