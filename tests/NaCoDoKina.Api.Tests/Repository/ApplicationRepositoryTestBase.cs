using NaCoDoKina.Api.Data;

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
    }
}