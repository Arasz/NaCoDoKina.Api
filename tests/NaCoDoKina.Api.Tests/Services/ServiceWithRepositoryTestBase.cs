using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace NaCoDoKina.Api.Services
{
    public abstract class ServiceTestBase<TService> : UnitTestBase
    {
        protected TService ServiceUnderTest { get; set; }

        protected Mock<ILogger<TService>> LoggerMock { get; set; }

        protected Mock<IMapper> MapperMock { get; set; }

        protected ServiceTestBase()
        {
            LoggerMock = new Mock<ILogger<TService>>();
            MapperMock = new Mock<IMapper>();
        }
    }

    public abstract class ServiceWithRepositoryTestBase<TService, TRepository> : ServiceTestBase<TService>
        where TRepository : class
    {
        protected Mock<TRepository> RepositoryMock { get; set; }

        protected TRepository RepositoryMockObject => RepositoryMock.Object;

        protected ServiceWithRepositoryTestBase()
        {
            RepositoryMock = new Mock<TRepository>();
        }
    }
}