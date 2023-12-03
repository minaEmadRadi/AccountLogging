
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Account_Loging.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(decimal id);
        T GetClaimDataUser(decimal Id);
        T Find(decimal id);
        Task<T> GetAsync(decimal id);
        Task<T> FindAsync(decimal id);
        T Get(Expression<Func<T, bool>> predicate);
        T Find(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        T Get(List<Expression<Func<T, bool>>> predicates);
        T Find(List<Expression<Func<T, bool>>> predicates);
        Task<T> GetAsync(List<Expression<Func<T, bool>>> predicates);
        Task<T> FindAsync(List<Expression<Func<T, bool>>> predicates);
        T GetLast<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> keySelector);
        T FindLast<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> keySelector);
        T GetLast<TKey>(List<Expression<Func<T, bool>>> predicates,
            Expression<Func<T, TKey>> keySelector);
        T FindLast<TKey>(List<Expression<Func<T, bool>>> predicates,
            Expression<Func<T, TKey>> keySelector);
        void Insert(T entity);
        void Add(T entity);
        void InsertRange(List<T> entities);
        void AddRange(List<T> entities);
        void Update(T entity, params string[] ignoredProperties);
        void Edit(T entity, params string[] ignoredProperties);
        void Delete(T entity);
        void Remove(T entity);
        void DeleteRange(List<T> entities);
        void RemoveRange(List<T> entities);
        void Attach(T entity);
        void Detach(T entity);
        
    }
}