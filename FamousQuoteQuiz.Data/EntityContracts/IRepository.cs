using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.EntityContracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        int Count();
        
        Task<int> CountAsync();

        int Count(Expression<Func<T, bool>> predicate);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        bool Any();

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> predicate);


        T FirstOrDefault();

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync();

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);


        T First();

        T First(Expression<Func<T, bool>> predicate);

        Task<T> FirstAsync();

        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);


        T SingleOrDefault();

        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> SingleOrDefaultAsync();

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);


        T Single();

        T Single(Expression<Func<T, bool>> predicate);

        Task<T> SingleAsync();

        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);


        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(T[] entity);

        Task AddRangeAsync(T[] entity);

        void Remove(int Id);

        void Remove(T entity);
    }
}
