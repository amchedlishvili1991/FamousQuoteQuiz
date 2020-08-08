using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.EntityContracts
{
    public interface IRepository<T1, T2>
        where T1 : class
        where T2 : class
    {
        IQueryable<T2> GetAll();

        IQueryable<T2> Where(Expression<Func<T1, bool>> predicate);

        int Count();
        
        Task<int> CountAsync();

        int Count(Expression<Func<T1, bool>> predicate);

        Task<int> CountAsync(Expression<Func<T1, bool>> predicate);

        bool Any();

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<T1, bool>> predicate);

        bool Any(Expression<Func<T1, bool>> predicate);


        T2 FirstOrDefault();

        T2 FirstOrDefault(Expression<Func<T1, bool>> predicate);

        Task<T2> FirstOrDefaultAsync();

        Task<T2> FirstOrDefaultAsync(Expression<Func<T1, bool>> predicate);


        T2 First();

        T2 First(Expression<Func<T1, bool>> predicate);

        Task<T2> FirstAsync();

        Task<T2> FirstAsync(Expression<Func<T1, bool>> predicate);


        T2 SingleOrDefault();

        T2 SingleOrDefault(Expression<Func<T1, bool>> predicate);

        Task<T2> SingleOrDefaultAsync();

        Task<T2> SingleOrDefaultAsync(Expression<Func<T1, bool>> predicate);


        T2 Single();

        T2 Single(Expression<Func<T1, bool>> predicate);

        Task<T2> SingleAsync();

        Task<T2> SingleAsync(Expression<Func<T1, bool>> predicate);


        void Add(T1 entity);

        Task AddAsync(T1 entity);

        void AddRange(T1[] entity);

        Task AddRangeAsync(T1[] entity);

        void Remove(int Id);

        void Remove(T1 entity);

        List<T2> ToList(IQueryable<T1> queryable);

        T2[] ToArray(IQueryable<T1> queryable);

        Task Update(T2 repositoryModel);

        IQueryable<T1> GetDbSet();
    }
}
