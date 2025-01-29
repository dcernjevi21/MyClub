using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityRepositories
{
    public abstract class Repository<T> : IDisposable where T : class
    {
        protected MyClubContext Context { get; set; }
        protected DbSet<T> Entities { get; set; }

        public Repository(MyClubContext context)
        {
            Context = context;
            Entities = context.Set<T>();
        }

        public virtual void Dispose()
        {
            Context.Dispose();
        }

        public virtual IQueryable<T> GetAll()
        {
            var query = from x in Entities select x;

            return query;
        }

        public virtual int Add(T entity, bool saveChanges = true)
        {
            Entities.Add(entity);
            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public abstract int Update(T entity, bool saveChanges = true);

        public virtual int Remove(T entity, bool saveChanges = true)
        {
            Entities.Attach(entity);
            Entities.Remove(entity);
            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public virtual int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
