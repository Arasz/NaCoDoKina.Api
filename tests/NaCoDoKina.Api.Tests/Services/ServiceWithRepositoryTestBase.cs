using Moq;

namespace NaCoDoKina.Api.Services
{
    public abstract class ServiceTestBase<TService>
    {
        protected TService ServiceUnderTest { get; set; }
    }

    public abstract class ServiceWithRepositoryTestBase<TService, TRepository> : ServiceTestBase<TService>
        where TRepository : class
    {
        protected Mock<TRepository> RepositoryMock { get; set; }

        protected TRepository RepositoryMockObject => RepositoryMock.Object;

        protected ServiceWithRepositoryTestBase()
        {
        }
    }
}