using Microsoft.Extensions.Logging;
using Moq;

namespace NaCoDoKina.Api.Repository
{
    public abstract class RepositoryTestBase<TRepository> : UnitTestBase
    {
        protected TRepository RepositoryUnderTest { get; set; }

        protected Mock<ILogger<TRepository>> LoggerMock { get; }

        protected InMemoryDatabaseScope DatabaseScope { get; }

        protected TestDatabaseContextScope CreateContextScope() => new TestDatabaseContextScope(DatabaseScope);

        protected RepositoryTestBase()
        {
            LoggerMock = new Mock<ILogger<TRepository>>();
            DatabaseScope = new InMemoryDatabaseScope();
            EnsureCreated();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                DatabaseScope.Dispose();
            }
        }

        /// <summary>
        /// Ensures that database schema is created 
        /// </summary>
        private void EnsureCreated()
        {
            using (var contextScope = new TestDatabaseContextScope(DatabaseScope))
            {
                contextScope.ApplicationContext.Database.EnsureCreated();
            }
        }
    }
}