using FamousQuoteQuiz.Data.Entity.Context;
using FamousQuoteQuiz.Data.EntityContracts;
using FamousQuoteQuiz.Data.Global;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.Entity.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public Repository(BaseDbContext dbContext)
        {
            DbContext = dbContext ?? throw new FqqException("DbContext is null");
            DbSet = DbContext.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void AddRange(T[] entity)
        {
            DbSet.AddRange(entity);
        }

        public async Task AddRangeAsync(T[] entity)
        {
            await DbSet.AddRangeAsync(entity);
        }

        public bool Any()
        {
            return DbSet.Any();
        }

        public async Task<bool> AnyAsync()
        {
            return await DbSet.AnyAsync();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public int Count()
        {
            return DbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }


        public T FirstOrDefault()
        {
            return DbSet.FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return await DbSet.FirstOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }


        public T First()
        {
            return DbSet.First();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return DbSet.First(predicate);
        }

        public async Task<T> FirstAsync()
        {
            return await DbSet.FirstAsync();
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstAsync(predicate);
        }


        public T SingleOrDefault()
        {
            return DbSet.SingleOrDefault();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public async Task<T> SingleOrDefaultAsync()
        {
            return await DbSet.SingleOrDefaultAsync();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }


        public T Single()
        {
            return DbSet.Single();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Single(predicate);
        }

        public async Task<T> SingleAsync()
        {
            return await DbSet.SingleAsync();
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleAsync(predicate);
        }


        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public void Remove(int Id)
        {
            var entity = DbSet.Find(Id);
            if (entity == null)
                throw new FqqException("Cannot find Record in Database");

            DbSet.Remove(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
    }
}
