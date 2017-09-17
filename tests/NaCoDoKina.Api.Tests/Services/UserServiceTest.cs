using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class UserServiceTest : ServiceTestBase<IUserService>
    {
        public UserServiceTest()
        {
            ServiceUnderTest = new UserService();
        }

        public class GetCurrentUserIdAsync : UserServiceTest
        {
            [Fact]
            public async Task Should_return_current_user_id()
            {
                //Arrange
                var expectedId = 1;

                //Act
                var userId = await ServiceUnderTest.GetCurrentUserIdAsync();

                //Assert
                userId.Should().Be(expectedId);
            }
        }
    }
}