using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class EfGenericRepository<T> : IRepository<T>
        where T : Entity
    {
        private readonly ApplicationContext _dbContext;

        public EfGenericRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<T> GetByIdAsync(long id) => _dbContext.Set<T>()
            .SingleOrDefaultAsync(e => e.Id == id);

        public Task<List<T>> ListAsync() => _dbContext.Set<T>()
            .ToListAsync();

        public Task<List<T>> ListAsync(ISpecification<T> specification)
        {
            var resultWithIncludes = specification.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            return resultWithIncludes
                .Where(specification.Criteria)
                .ToListAsync();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public void Update(T entity)
        {
            var entityEntry = _dbContext.Entry(entity);
            entityEntry.State = EntityState.Modified;
        }

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

        public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}