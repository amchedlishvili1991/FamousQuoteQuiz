using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class Repository<T1, T2> : IRepository<T1, T2>
        where T1 : class
        where T2 : class
    {
        protected DbContext DbContext { get; set; }

        protected DbSet<T1> DbSet { get; set; }

        /// <summary>
        /// mapper repository model to entity model
        /// </summary>
        private Mapper mapperToRepo;

        /// /// <summary>
        /// mapper entity model to repository model
        /// </summary>
        private Mapper mapperToEntity;

        private IConfigurationProvider entityToRepo;

        /// <summary>
        /// repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="repoToEntity"></param>
        /// <param name="entityToRepo"></param>
        public Repository(BaseDbContext dbContext,
            IConfigurationProvider repoToEntity,
            IConfigurationProvider entityToRepo)
        {
            mapperToEntity = new Mapper(repoToEntity);
            
            this.entityToRepo = entityToRepo;
            mapperToRepo = new Mapper(entityToRepo);
            
            DbContext = dbContext ?? throw new FqqException("DbContext is null");
            DbSet = DbContext.Set<T1>();
        }

        public void Add(T1 entity)
        {
            DbSet.Add(entity);
        }

        public async Task AddAsync(T1 entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void AddRange(T1[] entity)
        {
            DbSet.AddRange(entity);
        }

        public async Task AddRangeAsync(T1[] entity)
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

        public bool Any(Expression<Func<T1, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T1, bool>> predicate)
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

        public int Count(Expression<Func<T1, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T1, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }


        public T2 FirstOrDefault()
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.FirstOrDefault());
            return result;
        }

        public T2 FirstOrDefault(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.FirstOrDefault(predicate));
            return result;
        }

        public async Task<T2> FirstOrDefaultAsync()
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.FirstOrDefaultAsync());
            return result;
        }

        public async Task<T2> FirstOrDefaultAsync(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.FirstOrDefaultAsync(predicate));
            return result;
        }


        public T2 First()
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.First());
            return result;
        }

        public T2 First(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.First(predicate));
            return result;
        }

        public async Task<T2> FirstAsync()
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.FirstAsync());
            return result;
        }

        public async Task<T2> FirstAsync(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.FirstAsync(predicate));
            return result;
        }


        public T2 SingleOrDefault()
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.SingleOrDefault());
            return result;
        }

        public T2 SingleOrDefault(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.SingleOrDefault(predicate));
            return result;
        }

        public async Task<T2> SingleOrDefaultAsync()
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.SingleOrDefaultAsync());
            return result;
        }

        public async Task<T2> SingleOrDefaultAsync(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.SingleOrDefaultAsync(predicate));
            return result;
        }


        public T2 Single()
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.Single());
            return result;
        }

        public T2 Single(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(DbSet.Single(predicate));
            return result;
        }

        public async Task<T2> SingleAsync()
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.SingleAsync());
            return result;
        }

        public async Task<T2> SingleAsync(Expression<Func<T1, bool>> predicate)
        {
            var result = mapperToRepo.Map<T1, T2>(await DbSet.SingleAsync(predicate));
            return result;
        }


        public IQueryable<T2> GetAll()
        {
            return DbSet.ProjectTo<T2>(entityToRepo);
        }

        public void Remove(int Id)
        {
            var entity = DbSet.Find(Id);
            if (entity == null)
                throw new FqqException("Cannot find Record in Database");

            DbSet.Remove(entity);
        }

        public void Remove(T1 entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T2> Where(Expression<Func<T1, bool>> predicate)
        {
            return DbSet.Where(predicate).ProjectTo<T2>(entityToRepo);
        }

        public List<T2> ToList(IQueryable<T1> queryable)
        {
            var result = queryable.ProjectTo<T2>(entityToRepo);
            return result.ToList();
        }

        public T2[] ToArray(IQueryable<T1> queryable)
        {
            var result = queryable.ProjectTo<T2>(entityToRepo);
            return result.ToArray();
        }

        public async Task Update(T2 repositoryModel)
        {
            try
            {
                var entity = mapperToEntity.Map<T1>(repositoryModel);
                var key = entity.GetType().GetProperties().Single(x => x.Name == "Id");
                var dbEntity = await DbSet.FindAsync(key.GetValue(entity));

                var entityProperties = entity.GetType().GetProperties().Where(x => x.Name != "Id" && (x.PropertyType.IsValueType || x.PropertyType == typeof(string)));

                foreach (var item in entityProperties)
                {
                    var singleEntityFiled = entity.GetType().GetProperty(item.Name);
                    var singleDbEntityFiled = dbEntity.GetType().GetProperty(item.Name);

                    singleDbEntityFiled.SetValue(dbEntity, singleEntityFiled.GetValue(entity));
                }

                DbContext.Entry(dbEntity).State = EntityState.Modified;
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException("Cannot find record identifier", false, iox);
            }
        }

        public IQueryable<T1> GetDbSet()
        {
            return DbSet;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
