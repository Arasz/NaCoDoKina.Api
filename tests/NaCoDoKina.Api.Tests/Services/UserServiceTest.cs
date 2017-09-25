using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using Ploeh.AutoFixture;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class UserServiceTest : ServiceTestBase<IUserService>
    {
        public UserServiceTest()
        {
            ServiceUnderTest = Mock.Create<UserService>();

            CreateUserMappingsMock();
        }

        private void CreateUserMappingsMock()
        {
            Mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<User>(It.IsAny<ApplicationUser>()))
                .Returns(new Func<ApplicationUser, User>(user => new User
                {
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName
                }));

            Mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<ApplicationUser>(It.IsAny<User>()))
                .Returns(new Func<User, ApplicationUser>(user => new ApplicationUser
                {
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName
                }));
        }

        public class AuthenticateAsync : UserServiceTest
        {
            [Fact]
            public async Task Should_return_positive_result_when_password_is_correct()
            {
                // Arrange
                var user = Fixture.Create<User>();
                var password = Fixture.Create<string>();
                var internalPasswordHash = Fixture.Create<string>();
                var internalUser = Fixture.Build<ApplicationUser>()
                    .With(applicationUser => applicationUser.PasswordHash, internalPasswordHash)
                    .Create();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByIdAsync(user.Id))
                    .ReturnsAsync(internalUser);

                Mock.Mock<IPasswordHasher<ApplicationUser>>()
                    .Setup(hasher => hasher.VerifyHashedPassword(internalUser, internalPasswordHash, password))
                    .Returns(PasswordVerificationResult.Success);

                // Act
                var result = await ServiceUnderTest.AuthenticateAsync(user, password);

                // Assert
                result.Succeeded.Should().BeTrue();
            }

            [Fact]
            public async Task Should_return_negative_result_when_password_is_wrong()
            {
                // Arrange
                var user = Fixture.Create<User>();
                var password = Fixture.Create<string>();
                var internalPasswordHash = Fixture.Create<string>();
                var internalUser = Fixture.Build<ApplicationUser>()
                    .With(applicationUser => applicationUser.PasswordHash, internalPasswordHash)
                    .Create();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByIdAsync(user.Id))
                    .ReturnsAsync(internalUser);

                Mock.Mock<IPasswordHasher<ApplicationUser>>()
                    .Setup(hasher => hasher.VerifyHashedPassword(internalUser, internalPasswordHash, password))
                    .Returns(PasswordVerificationResult.Failed);

                // Act
                var result = await ServiceUnderTest.AuthenticateAsync(user, password);

                // Assert
                result.Succeeded.Should().BeFalse();
            }

            [Fact]
            public async Task Should_return_positive_result_and_rehash_password_when_needed()
            {
                // Arrange
                var user = Fixture.Create<User>();
                var password = Fixture.Create<string>();
                var internalPasswordHash = Fixture.Create<string>();
                var internalUser = Fixture.Build<ApplicationUser>()
                    .With(applicationUser => applicationUser.PasswordHash, internalPasswordHash)
                    .Create();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByIdAsync(user.Id))
                    .ReturnsAsync(internalUser);

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.UpdateUserPassword(internalUser, password, password))
                    .ReturnsAsync(true)
                    .Verifiable();

                Mock.Mock<IPasswordHasher<ApplicationUser>>()
                    .Setup(hasher => hasher.VerifyHashedPassword(internalUser, internalPasswordHash, password))
                    .Returns(PasswordVerificationResult.SuccessRehashNeeded);

                // Act
                var result = await ServiceUnderTest.AuthenticateAsync(user, password);

                // Assert
                result.Succeeded.Should().BeTrue();
                Mock.Mock<IUserRepository>().Verify();
            }

            [Fact]
            public async Task Should_return_negative_result_when_password_rehash_failed()
            {
                // Arrange
                var user = Fixture.Create<User>();
                var password = Fixture.Create<string>();
                var internalPasswordHash = Fixture.Create<string>();
                var internalUser = Fixture.Build<ApplicationUser>()
                    .With(applicationUser => applicationUser.PasswordHash, internalPasswordHash)
                    .Create();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByIdAsync(user.Id))
                    .ReturnsAsync(internalUser);

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.UpdateUserPassword(internalUser, password, password))
                    .ReturnsAsync(false)
                    .Verifiable();

                Mock.Mock<IPasswordHasher<ApplicationUser>>()
                    .Setup(hasher => hasher.VerifyHashedPassword(internalUser, internalPasswordHash, password))
                    .Returns(PasswordVerificationResult.SuccessRehashNeeded);

                // Act
                var result = await ServiceUnderTest.AuthenticateAsync(user, password);

                // Assert
                result.Succeeded.Should().BeFalse();
                Mock.Mock<IUserRepository>().Verify();
            }
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