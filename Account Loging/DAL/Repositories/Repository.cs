
using Account_Loging.DAL.Repositories;
using AccountLog.DAL.DbContainer;
using Microsoft.EntityFrameworkCore;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace MatrixTask.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AccountLogContext _context;
        private DbSet<T> _dbSet;

        public Repository(AccountLogContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public List<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public List<T> GetAll(List<Expression<Func<T, bool>>> predicates)
        {
            IQueryable<T> queryable = _dbSet;

            foreach (var predicate in predicates)
            {
                queryable = queryable.Where(predicate);
            }

            return queryable?.ToList();
        }

        public async Task<List<T>> GetAllAsync(List<Expression<Func<T, bool>>> predicates)
        {
            IQueryable<T> queryable = _dbSet;

            foreach (var predicate in predicates)
            {
                queryable = queryable.Where(predicate);
            }

            return await queryable?.ToListAsync();
        }

        

        public T Get(decimal id)
        {
            return _dbSet.Find(id);
        }

        public T Find(decimal id)
        {
            return Get(id);
        }

        public async Task<T> GetAsync(decimal id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> FindAsync(decimal id)
        {
            return await GetAsync(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return Get(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAsync(predicate);
        }

        public T Get(List<Expression<Func<T, bool>>> predicates)
        {
            IQueryable<T> queryable = _dbSet;

            foreach (var predicate in predicates)
            {
                queryable = queryable.Where(predicate);
            }

            return queryable?.FirstOrDefault();
        }

        public T Find(List<Expression<Func<T, bool>>> predicates)
        {
            return Get(predicates);
        }

        public async Task<T> GetAsync(List<Expression<Func<T, bool>>> predicates)
        {
            IQueryable<T> queryable = _dbSet;

            foreach (var predicate in predicates)
            {
                queryable = queryable.Where(predicate);
            }

            return await queryable?.FirstOrDefaultAsync();
        }

        public async Task<T> FindAsync(List<Expression<Func<T, bool>>> predicates)
        {
            return await GetAsync(predicates);
        }

        //LastOrDefault not works, so we need to order by descending, then get first item
        public T GetLast<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> keySelector)
        {
            return _dbSet.Where(predicate).OrderByDescending(keySelector)
                .FirstOrDefault();
        }

        public T FindLast<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> keySelector)
        {
            return GetLast(predicate, keySelector);
        }

        public T GetLast<TKey>(List<Expression<Func<T, bool>>> predicates,
            Expression<Func<T, TKey>> keySelector)
        {
            IQueryable<T> queryable = _dbSet;

            foreach (var predicate in predicates)
            {
                queryable = queryable.Where(predicate);
            }

            return queryable?.OrderByDescending(keySelector).FirstOrDefault();
        }

        public T FindLast<TKey>(List<Expression<Func<T, bool>>> predicates,
            Expression<Func<T, TKey>> keySelector)
        {
            return GetLast(predicates, keySelector);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _dbSet.Add(entity);
        }

        public void Add(T entity)
        {
            Insert(entity);
        }

        public void InsertRange(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            _dbSet.AddRange(entities);
        }

        public void AddRange(List<T> entities)
        {
            InsertRange(entities);
        }


        public void Update(T entity, params string[] ignoredProperties)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _context.Entry(entity).State = EntityState.Modified;

            foreach (var ignoredProperty in ignoredProperties)
            {
                _context.Entry(entity).Property(ignoredProperty).IsModified = false;
            }
        }

        public void Edit(T entity, params string[] ignoredProperties)
        {
            Update(entity, ignoredProperties);
        }

        public void Attach(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _dbSet.Attach(entity);
        }

        public void Detach(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _context.Entry(entity).State = EntityState.Detached;
        }


        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
        }

        public void Remove(T entity)
        {
            Delete(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            _dbSet.RemoveRange(entities);
        }

        public void RemoveRange(List<T> entities)
        {
            DeleteRange(entities);
        }

        public T GetClaimDataUser(decimal Id)
        {
            throw new NotImplementedException();
        }
    }
}