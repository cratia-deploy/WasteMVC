using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WasteMVC.Models;

namespace WasteMVC.Data
{
    public class Repository<TContext, TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
        where TContext : DbContext
    {
        /// <summary>
        /// Patrón Singleton
        /// </summary>
        private static Repository<TContext, TEntity> instance = null;
        private static readonly object padlock = new object();

        private readonly TContext Context = null;
        private readonly DbSet<TEntity> EntitySet = null;

        public Repository(TContext _DbContext)
        {
            if (_DbContext != null)
            {
                this.Context = _DbContext;
                this.EntitySet = this.Context.Set<TEntity>();
            }
        }

        public static Repository<TContext, TEntity> Instance(TContext _DbContext)
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Repository<TContext, TEntity>(_DbContext);
                    }
                }
            }
            return instance;
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return null;
            }
            IQueryable<TEntity> query = this.EntitySet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }
        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return null;
            }
            else if (filter != null)
            {
                return this.EntitySet.Where(filter).ToList();
            }
            else
            {
                return this.EntitySet.ToList();
            }
        }

        public virtual bool Add(TEntity _object)
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return false;
            }
            else if ((_object is TEntity) && (_object != null))
            {
                EntitySet.Add(_object);
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool Add(List<TEntity> _objects)
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return false;
            }
            else if ((_objects is List<TEntity>) && (_objects != null))
            {
                EntitySet.AddRange(_objects);
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual TEntity Find(int _id)
        {
            if ((this.Context == null) || (this.EntitySet == null) || (_id < 0))
            {
                return null;
            }
            else
            {
                return this.EntitySet.FirstOrDefault(x => x.Id == _id);
            }
        }
        public virtual TEntity Find(Expression<Func<TEntity, bool>> filter)
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return null;
            }
            else if (filter != null)
            {
                return this.EntitySet.FirstOrDefault(filter);
            }
            else
            {
                return null;
            }
        }
        public virtual TEntity Firts()
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return null;
            }
            else
            {
                return this.EntitySet.FirstOrDefault();
            }
        }
        public virtual TEntity Last()
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return null;
            }
            else
            {
                return this.EntitySet.LastOrDefault();
            }
        }
        public virtual bool Delete(int _id)
        {
            if ((this.Context == null) || (this.EntitySet == null) || (_id < 0))
            {
                return false;
            }
            else
            {
                TEntity _data = this.EntitySet.Find(_id);
                if (_data == null)
                    return false;
                else
                {
                    this.EntitySet.Remove(_data);
                    return true;
                }
            }
        }
        public virtual bool Delete(Expression<Func<TEntity, bool>> filter)
        {
            if ((this.Context == null) || (this.EntitySet == null) || (filter == null))
            {
                return false;
            }
            else
            {
                var data = this.EntitySet.Where(filter);
                if (data.Count() == 0)
                {
                    return false;
                }
                else
                {
                    this.EntitySet.RemoveRange(data);
                    return true;
                }
            }
        }
        public virtual bool Update(TEntity _objectupdate)
        {
            if ((this.Context == null) || (this.EntitySet == null) || (_objectupdate == null))
            {
                return false;
            }
            else
            {
                this.EntitySet.Attach(_objectupdate);
                this.Context.Entry(_objectupdate).State = EntityState.Modified;
                return true;
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            if ((this.Context == null) || (this.EntitySet == null) || (filter == null))
            {
                return false;
            }
            else
            {
                return this.EntitySet.Any(filter);
            }
        }

        // Metodos Asycn
        public virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return null;
            }
            else if (filter != null)
            {
                return this.EntitySet.Where(filter).ToListAsync();
            }
            else
            {
                return this.EntitySet.ToListAsync();
            }
        }
        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            if ((this.Context == null) || (this.EntitySet == null))
            {
                return null;
            }
            else if (filter != null)
            {
                return this.EntitySet.FirstOrDefaultAsync(filter);
            }
            else
            {
                return null;
            }
        }
    }
}