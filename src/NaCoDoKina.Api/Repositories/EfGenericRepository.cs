using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class EfGenericRepository<T> : IRepository<T>
        where T : Entity
    {
        public Task<T> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> ListAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> AddAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}