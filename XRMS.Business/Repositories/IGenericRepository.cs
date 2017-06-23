using System;
using System.Collections.Generic;

using System.Linq.Expressions;

namespace XRMS.Business.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        //IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);

        T Add(T entity);

        T Remove(T entity);

        void Update(T entity);

        //void Save();
    }
}

/*namespace Data.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}*/
