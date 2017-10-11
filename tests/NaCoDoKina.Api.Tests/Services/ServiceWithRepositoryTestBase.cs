using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TestsCore;

namespace NaCoDoKina.Api.Services
{
    public abstract class ServiceTestBase<TService> : UnitTestBase
    {
        protected TService ServiceUnderTest => Mock.Create<TService>();

        protected Mock<ILogger<TService>> LoggerMock => Mock.Mock<ILogger<TService>>();

        protected Mock<IMapper> MapperMock => Mock.Mock<IMapper>();
    }

    public abstract class ServiceWithRepositoryTestBase<TService, TRepository> : ServiceTestBase<TService>
        where TRepository : class
    {
        protected Mock<TRepository> RepositoryMock => Mock.Mock<TRepository>();
    }
}