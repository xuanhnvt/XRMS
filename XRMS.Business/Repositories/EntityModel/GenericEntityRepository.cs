using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace XRMS.Business.Repositories.EntityModel
{
    public class GenericEntityRepository<TEntityModel> : IGenericRepository<TEntityModel> where TEntityModel : class
    {
        protected DbContext Context { get; set; }

        public GenericEntityRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            Context = dbContext;
            //DbSet = DbContext.Set<T>();
        }

        public virtual IEnumerable<TEntityModel> GetAll()
        {
            IEnumerable<TEntityModel> query = Context.Set<TEntityModel>();
            return query;
        }

        /*public IQueryable<TEntityModel> GetBy(System.Linq.Expressions.Expression<Func<TEntityModel, bool>> predicate)
        {
            IQueryable<TEntityModel> query = Context.Set<TEntityModel>().Where(predicate);
            return query;
        }*/

        public virtual IEnumerable<TEntityModel> GetBy(System.Linq.Expressions.Expression<Func<TEntityModel, bool>> predicate)
        {
            IEnumerable<TEntityModel> query = Context.Set<TEntityModel>().Where(predicate);
            return query;
        }

        public virtual TEntityModel Find(params object[] keys)
        {
            return Context.Set<TEntityModel>().Find(keys);
        }

        public virtual TEntityModel Add(TEntityModel entity)
        {
            return Context.Set<TEntityModel>().Add(entity);
        }

        public virtual TEntityModel Remove(TEntityModel entity)
        {
            return Context.Set<TEntityModel>().Remove(entity);
        }

        public virtual void Update(TEntityModel entity)
        {
            //Context.Entry(entity).State = EntityState.Added;
            //Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            Context.Set<TEntityModel>().AddOrUpdate(entity);
            //Context.Set<TEntityModel>().Add(entity);
            //Context.Set<TEntityModel>().

            /*var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached || entry.State == EntityState.Modified)
            {
                Context.Set<TEntityModel>().Attach(entity); //attach
                entry.State = EntityState.Modified; //do it here

                Context.SaveChanges(); //save it
            }*/
        }

        /*public virtual void Save()
        {
            Context.SaveChanges();
        }*/
    }
}
