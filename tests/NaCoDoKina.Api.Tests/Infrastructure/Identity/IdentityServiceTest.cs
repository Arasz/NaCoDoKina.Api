using Autofac.Extras.Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Token;
using Ploeh.AutoFixture;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using AuthenticationToken = NaCoDoKina.Api.Infrastructure.Services.Token.AuthenticationToken;

namespace NaCoDoKina.Api.Infrastructure.Identity
{
    public class IdentityServiceTest
    {
        public Fixture Fixture { get; }

        public IdentityServiceTest()
        {
            Fixture = new Fixture();
        }

        public class LoginAsync : IdentityServiceTest
        {
            [Fact]
            public async Task Should_return_token_in_success_result_when_user_logged_in()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var username = Fixture.Create<string>();
                    var password = Fixture.Create<string>();
                    var userId = Fixture.Create<long>();
                    var token = Fixture.Create<AuthenticationToken>();

                    mock.Mock<ISignInManager>()
                        .Setup(manager => manager.PasswordSignInAsync(username, password))
                        .ReturnsAsync(() => SignInResult.Success);

                    mock.Mock<IUserManager>()
                        .Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>()))
                        .Returns(userId.ToString);

                    mock.Mock<ITokenService>()
                        .Setup(service => service.CreateToken(It.IsAny<UserInformation>()))
                        .Returns(() => token);

                    //Act
                    var result = await mock.Create<IdentityService>()
                        .LoginAsync(username, password);

                    var returnedToken = result.Data;

                    //Assert

                    returnedToken.Should().Be(token);
                }
            }

            [Fact]
            public async Task Should_return_failed_result_when_user_not_logged_in()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var username = Fixture.Create<string>();
                    var password = Fixture.Create<string>();
                    var userId = Fixture.Create<long>();
                    var token = Fixture.Create<AuthenticationToken>();

                    mock.Mock<ISignInManager>()
                        .Setup(manager => manager.PasswordSignInAsync(username, password))
                        .ReturnsAsync(() => SignInResult.Failed);

                    mock.Mock<IUserManager>()
                        .Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>()))
                        .Returns(userId.ToString);

                    mock.Mock<ITokenService>()
                        .Setup(service => service.CreateToken(It.IsAny<UserInformation>()))
                        .Returns(() => token);

                    //Act
                    var result = await mock.Create<IdentityService>()
                        .LoginAsync(username, password);

                    //Assert

                    result.Data.Should().BeNull();
                    result.Succeeded.Should().BeFalse();
                    result.FailReason.Should().NotBeNullOrEmpty();
                }
            }
        }

        public class GetUserByNameAsync : IdentityServiceTest
        {
            [Fact]
            public async Task Should_return_application_user_with_given_name_when_user_exist()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var username = Fixture.Create<string>();
                    var applicationUser = Fixture.Build<ApplicationUser>()
                        .With(user => user.UserName, username)
                        .With(user => user.Email, username)
                        .Create();

                    mock.Mock<IUserManager>()
                        .Setup(manager => manager.GetUserByNameAsync(username))
                        .ReturnsAsync(applicationUser);

                    //Act
                    var returnedUser = await mock.Create<IdentityService>()
                        .GetUserByNameAsync(username);

                    //Assert

                    returnedUser.Should().Be(applicationUser);
                }
            }

            [Fact]
            public async Task Should_return_null_when_user_do_not_exist()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var username = Fixture.Create<string>();

                    mock.Mock<IUserManager>()
                        .Setup(manager => manager.GetUserByNameAsync(username))
                        .ReturnsAsync((ApplicationUser)null);

                    //Act
                    var returnedUser = await mock.Create<IdentityService>()
                        .GetUserByNameAsync(username);

                    //Assert

                    returnedUser.Should().BeNull();
                }
            }
        }

        public class CreateUserAsync : IdentityServiceTest
        {
            [Fact]
            public async Task Should_return_success_result_and_sing_in_when_created()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var password = Fixture.Create<string>();
                    var username = Fixture.Create<string>();
                    var applicationUser = Fixture.Build<ApplicationUser>()
                        .With(user => user.UserName, username)
                        .With(user => user.Email, username)
                        .Create();

                    mock.Mock<IUserManager>()
                        .Setup(manager => manager.CreateAsync(applicationUser, password))
                        .ReturnsAsync(() => IdentityResult.Success)
                        .Verifiable();

                    mock.Mock<ISignInManager>()
                        .Setup(manager => manager.SignInAsync(applicationUser))
                        .Returns(Task.CompletedTask)
                        .Verifiable();

                    //Act
                    var returnedUser = await mock.Create<IdentityService>()
                        .CreateUserAsync(applicationUser, password);

                    //Assert
                    returnedUser.Succeeded.Should().BeTrue();

                    mock.Mock<IUserManager>().Verify();
                    mock.Mock<ISignInManager>().Verify();
                }
            }

            [Fact]
            public async Task Should_return_failure_result_and_do_not_sing_in()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var password = Fixture.Create<string>();
                    var username = Fixture.Create<string>();
                    var applicationUser = Fixture.Build<ApplicationUser>()
                        .With(user => user.UserName, username)
                        .With(user => user.Email, username)
                        .Create();

                    mock.Mock<IUserManager>()
                        .Setup(manager => manager.CreateAsync(applicationUser, password))
                        .ReturnsAsync(() => IdentityResult.Failed(new IdentityError()))
                        .Verifiable();

                    mock.Mock<ISignInManager>()
                        .Setup(manager => manager.SignInAsync(applicationUser))
                        .Returns(Task.CompletedTask);

                    //Act
                    var returnedUser = await mock.Create<IdentityService>()
                        .CreateUserAsync(applicationUser, password);

                    //Assert
                    returnedUser.Succeeded.Should().BeFalse();

                    mock.Mock<IUserManager>().Verify();
                    mock.Mock<ISignInManager>().Verify(manager => manager.SignInAsync(applicationUser), Times.Never);
                }
            }
        }

        public class VerifyPassword : IdentityServiceTest
        {
            [Fact]
            public void Should_return_true_when_password_is_correct()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var password = Fixture.Create<string>();
                    var passwordHash = Fixture.Create<string>();
                    var applicationUser = Fixture.Build<ApplicationUser>()
                        .With(user => user.PasswordHash, passwordHash)
                        .Create();

                    mock.Mock<IPasswordHasher<ApplicationUser>>()
                        .Setup(hasher => hasher.VerifyHashedPassword(applicationUser, passwordHash, password))
                        .Returns(PasswordVerificationResult.Success);

                    //Act
                    var isPasswordVerified = mock.Create<IdentityService>().VerifyPassword(applicationUser, password);

                    //Assert

                    isPasswordVerified.Should().Be(true);
                }
            }

            [Fact]
            public void Should_return_false_when_password_is_not_correct()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    //Arrange
                    var password = Fixture.Create<string>();
                    var passwordHash = Fixture.Create<string>();
                    var applicationUser = Fixture.Build<ApplicationUser>()
                        .With(user => user.PasswordHash, passwordHash)
                        .Create();

                    mock.Mock<IPasswordHasher<ApplicationUser>>()
                        .Setup(hasher => hasher.VerifyHashedPassword(applicationUser, passwordHash, password))
                        .Returns(PasswordVerificationResult.Failed);

                    //Act
                    var isPasswordVerified = mock.Create<IdentityService>().VerifyPassword(applicationUser, password);

                    //Assert

                    isPasswordVerified.Should().Be(false);
                }
            }
        }
    }
}