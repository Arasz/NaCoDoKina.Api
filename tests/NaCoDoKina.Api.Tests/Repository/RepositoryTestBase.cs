using Microsoft.Extensions.Logging;
using Moq;

namespace NaCoDoKina.Api.Repository
{
    public abstract partial class RepositoryTestBase<TRepository>
    {
        protected TRepository RepositoryUnderTest { get; set; }

        protected Mock<ILogger<TRepository>> LoggerMock { get; }

        protected RepositoryTestBase()
        {
            LoggerMock = new Mock<ILogger<TRepository>>();
        }

        /// <summary>
        /// Ensures that database schema is created 
        /// </summary>
        /// <param name="databaseScope"> Database scope </param>
        protected void EnsureCreated(InMemoryDatabaseScope databaseScope)
        {
            using (var contextScope = new TestDatabaseContextScope(databaseScope))
            {
                contextScope.ApplicationContext.Database.EnsureCreated();
            }
        }
    }
}