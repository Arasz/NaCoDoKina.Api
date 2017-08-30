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
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<List<T>> ListAsync(ISpecification<T> spec);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}