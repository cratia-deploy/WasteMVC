using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteMVC.Models;
using System.Threading.Tasks;

namespace WasteMVC.Data
{
    class UnitOfWork<TContext> : IDisposable
            where TContext : DbContext, new()
    {
        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private TContext Context = null;
        private readonly Dictionary<Type, object> Repositories = null;

        public UnitOfWork()
        {
            this.Context = new TContext();
            this.Repositories = new Dictionary<Type, object>();
        }

        public UnitOfWork(TContext _context)
        {
            if (_context != null)
            {
                this.Context = _context;
                this.Repositories = new Dictionary<Type, object>();
            }
        }

        public virtual IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase
        {
            if (!Repositories.Keys.Contains(typeof(TEntity)))
            {
                this.Repositories.Add(
                    typeof(TEntity),
                    new Repository<TContext, TEntity>(ref this.Context)
                    );
            }
            return Repositories[typeof(TEntity)] as IRepository<TEntity>; ;
        }

        private int Save()
        {
            bool saveFailed;
            int count = 0;

            foreach (var entry in this.Context.ChangeTracker.Entries())
            {
                EntityBase entity = (EntityBase)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.Created_At = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.Updated_At = DateTime.Now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entity.Deleted_At = DateTime.Now;
                }
            }

            do
            {
                saveFailed = false;
                try
                {
                    count += this.Context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    saveFailed = true;
                    var _entry = Context.ChangeTracker.Entries().First();
                    this.RollBack(_entry);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
                catch (Exception ex)
                {
                    saveFailed = true;
                    this.RollBack();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } while (saveFailed);
            return count;
        }

        private void RollBack(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                default:
                    break;
            }
        }

        public IEnumerable<object> RollBack()
        {
            List<EntityEntry> _data = Context.ChangeTracker.Entries()
                .Where(x => (x.State != EntityState.Unchanged) || (x.State != EntityState.Detached))
                .ToList();
            foreach (var entry in _data)
            {
                this.RollBack(entry);
            }
            List<object> _dataObject = new List<object>();
            foreach (var item in _data)
            {
                _dataObject.Add(item.CurrentValues.ToObject());
            }
            return _dataObject;
        }

        public IEnumerable<TEntity> RollBack<TEntity>()
            where TEntity : EntityBase
        {
            List<EntityEntry> _data = Context.ChangeTracker.Entries()
                .Where(
                    x => (
                        (x.Entity.GetType() == typeof(TEntity))
                     ))
                .ToList();
            foreach (var entry in _data)
            {
                this.RollBack(entry);
            }
            List<TEntity> _dataObject = new List<TEntity>();
            foreach (var item in _data)
            {
                _dataObject.Add(((TEntity)(item.CurrentValues.ToObject())));
            }
            return _dataObject;
        }

        internal virtual int Commit()
        {
            return this.Save();
        }

        private Task<int> SaveAsync()
        {
            foreach (var entry in this.Context.ChangeTracker.Entries())
            {
                EntityBase entity = (EntityBase)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.Created_At = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.Updated_At = DateTime.Now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entity.Deleted_At = DateTime.Now;
                }
            }
            return this.Context.SaveChangesAsync();
        }

        internal Task<int> CommitAsync()
        {
            return this.SaveAsync();
        }
    }
}