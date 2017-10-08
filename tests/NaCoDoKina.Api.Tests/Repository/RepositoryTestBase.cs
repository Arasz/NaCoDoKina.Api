using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NaCoDoKina.Api.Repository.Database;

namespace NaCoDoKina.Api.Repository
{
    /// <inheritdoc/>
    /// <summary>
    /// Base class for all repositories tests 
    /// </summary>
    /// <typeparam name="TRepository"> Repository under test type </typeparam>
    /// <typeparam name="TDbContext"> Context used by repository </typeparam>
    public abstract class RepositoryTestBase<TRepository, TDbContext> : UnitTestBase
        where TDbContext : DbContext
    {
        protected TRepository RepositoryUnderTest { get; set; }

        protected Mock<ILogger<TRepository>> LoggerMock => Mock.Mock<ILogger<TRepository>>();

        protected InMemoryDatabaseScope InMemoryDatabaseScope { get; }

        protected virtual DbContextScope<TDbContext> CreateContextScope()
        {
            return new DbContextScope<TDbContext>(InMemoryDatabaseScope);
        }

        protected RepositoryTestBase()
        {
            InMemoryDatabaseScope = new InMemoryDatabaseScope();
            EnsureCreated();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                InMemoryDatabaseScope.Dispose();
            }
        }

        /// <summary>
        /// Ensures that database schema is created 
        /// </summary>
        private void EnsureCreated()
        {
            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Database.EnsureCreated();
            }
        }
    }
}