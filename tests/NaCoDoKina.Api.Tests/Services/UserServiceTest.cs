using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class UserServiceTest : ServiceTestBase<IUserService>
    {
        public UserServiceTest()
        {
            ServiceUnderTest = Mock.Create<UserService>();
        }

        public class GetCurrentUserIdAsync : UserServiceTest
        {
            [Fact]
            public void Should_return_current_user_id()
            {
                //Arrange
                var expectedId = 1;

                Mock.Mock<IAuthenticatedUserId>()
                    .Setup(authenticatedUserId => authenticatedUserId.Id)
                    .Returns(expectedId);

                //Act
                var userId = ServiceUnderTest.GetCurrentUserId();

                //Assert
                userId.Should().Be(expectedId);
            }
        }
    }
}