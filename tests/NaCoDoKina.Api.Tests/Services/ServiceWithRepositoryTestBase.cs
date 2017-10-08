using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace NaCoDoKina.Api.Services
{
    public abstract class ServiceTestBase<TService> : UnitTestBase
    {
        protected TService ServiceUnderTest { get; set; }

        protected Mock<ILogger<TService>> LoggerMock => Mock.Mock<ILogger<TService>>();

        protected Mock<IMapper> MapperMock => Mock.Mock<IMapper>();

        protected ServiceTestBase()
        {
            ServiceUnderTest = Mock.Create<TService>();
        }
    }

    public abstract class ServiceWithRepositoryTestBase<TService, TRepository> : ServiceTestBase<TService>
        where TRepository : class
    {
        protected Mock<TRepository> RepositoryMock => Mock.Mock<TRepository>();

        protected TRepository RepositoryMockObject => RepositoryMock.Object;
    }
}