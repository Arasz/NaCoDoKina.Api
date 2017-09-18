using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class UserServiceTest : ServiceTestBase<IUserService>
    {
        protected Mock<IIdentityService> IdentityServiceMock { get; }

        public UserServiceTest()
        {
            IdentityServiceMock = new Mock<IIdentityService>();
            ServiceUnderTest = new UserService(IdentityServiceMock.Object);
        }

        public class GetCurrentUserIdAsync : UserServiceTest
        {
            [Fact]
            public async Task Should_return_current_user_id()
            {
                //Arrange
                var expectedId = 1;

                IdentityServiceMock
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(expectedId);

                //Act
                var userId = await ServiceUnderTest.GetCurrentUserIdAsync();

                //Assert
                userId.Should().Be(expectedId);
            }
        }
    }
}