using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Repository.Database;

namespace NaCoDoKina.Api.Repository
{
    /// <inheritdoc/>
    /// <summary>
    /// Base class for test which are using Application Context 
    /// </summary>
    /// <typeparam name="TRepository"> Tested repository type </typeparam>
    public class ApplicationRepositoryTestBase<TRepository>
        : RepositoryTestBase<TRepository, ApplicationContext>
    {
        protected override DbContextScope<ApplicationContext> CreateContextScope()
        {
            var contextScope = new DbContextScope<ApplicationContext>(InMemoryDatabaseScope);
            Mock.Provide(contextScope.DbContext);
            RepositoryUnderTest = Mock.Create<TRepository>();
            return contextScope;
        }
    }
}