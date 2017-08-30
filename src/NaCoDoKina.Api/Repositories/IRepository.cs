using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    /// <summary>
    /// Generic repository interface for business models 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(int id);

        Task<List<T>> ListAsync();

        /// <summary>
        /// Lists entities that satisfies given specification 
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<List<T>> ListAsync(ISpecification<T> specification);

        T Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        /// <summary>
        /// Implementation of unit of work 
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}