using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    .With(applicationUser => applicationUser.Id, user.Id)
                    .With(applicationUser => applicationUser.PasswordHash, internalPasswordHash)
                    .Create();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByNameAsync(user.UserName))
                    .ReturnsAsync(internalUser);

                Mock.Mock<IPasswordHasher<ApplicationUser>>()
                    .Setup(hasher => hasher.VerifyHashedPassword(internalUser, internalPasswordHash, password))
                    .Returns(PasswordVerificationResult.Success);

                // Act
                var result = await ServiceUnderTest.AuthenticateAsync(user, password);

                // Assert
                result.Succeeded.Should().BeTrue();
                result.Data.Id.Should().Be(user.Id);
            }

            [Fact]
            public async Task Should_return_negative_result_when_user_can_not_be_found()
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
                    .ReturnsAsync((ApplicationUser)null);

                Mock.Mock<IPasswordHasher<ApplicationUser>>()
                    .Setup(hasher => hasher.VerifyHashedPassword(internalUser, internalPasswordHash, password))
                    .Returns(PasswordVerificationResult.Success);

                // Act
                var result = await ServiceUnderTest.AuthenticateAsync(user, password);

                // Assert
                result.Succeeded.Should().BeFalse();
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
                    .Setup(repository => repository.GetUserByNameAsync(user.UserName))
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
                    .Setup(repository => repository.GetUserByNameAsync(user.UserName))
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

        public class GetAllUsersAsync : UserServiceTest
        {
            [Fact]
            public async Task Should_return_all_users()
            {
                // Arrange
                var usersCount = 10;
                var internalUsers = Fixture.CreateMany<ApplicationUser>(usersCount);

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetAllUsersAsync())
                    .ReturnsAsync(internalUsers);

                // Act
                var returnedUsers = await ServiceUnderTest.GetAllUsersAsync();

                // Assert
                returnedUsers.Should().HaveSameCount(internalUsers);
                returnedUsers
                    .Join(internalUsers, user => user.Id, user => user.Id,
                        (user, applicationUser) => (user.Id, applicationUser.Id))
                    .Should().HaveCount(usersCount);
            }

            [Fact]
            public void Should_throw_user_not_found_exception_when_there_is_no_user_to_return()
            {
                // Arrange
                var usersCount = 0;
                var internalUsers = Fixture.CreateMany<ApplicationUser>(usersCount);

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetAllUsersAsync())
                    .ReturnsAsync(internalUsers);

                // Act
                Func<Task<IEnumerable<User>>> action = () => ServiceUnderTest.GetAllUsersAsync();

                // Assert
                action.ShouldThrow<UserNotFoundException>();
            }
        }

        public class GetUserByNameAsync : UserServiceTest
        {
            [Fact]
            public async Task Should_return_user_with_given_name()
            {
                // Arrange
                var internalUser = Fixture.Create<ApplicationUser>();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByNameAsync(internalUser.UserName))
                    .ReturnsAsync(internalUser);

                // Act
                var returnedUser = await ServiceUnderTest.GetUserByNameAsync(internalUser.UserName);

                // Assert
                returnedUser
                    .Should().NotBeNull();
                returnedUser.UserName.Should().Be(internalUser.UserName);
            }

            [Fact]
            public void Should_throw_user_not_found_exception_when_there_is_no_user_to_return()
            {
                // Arrange
                var internalUser = Fixture.Create<ApplicationUser>();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByNameAsync(internalUser.UserName))
                    .ReturnsAsync((ApplicationUser)null);

                // Act
                Func<Task<User>> action = () => ServiceUnderTest.GetUserByNameAsync(internalUser.UserName);

                // Assert
                action.ShouldThrow<UserNotFoundException>();
            }
        }

        public class GetUserByEmailAsync : UserServiceTest
        {
            [Fact]
            public async Task Should_return_user_with_given_email()
            {
                // Arrange
                var internalUser = Fixture.Create<ApplicationUser>();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByEmailAsync(internalUser.Email))
                    .ReturnsAsync(internalUser);

                // Act
                var returnedUser = await ServiceUnderTest.GetUserByEmailAsync(internalUser.Email);

                // Assert
                returnedUser
                    .Should().NotBeNull();
                returnedUser.Email.Should().Be(internalUser.Email);
            }

            [Fact]
            public void Should_throw_user_not_found_exception_when_there_is_no_user_to_return()
            {
                // Arrange
                var internalUser = Fixture.Create<ApplicationUser>();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.GetUserByEmailAsync(internalUser.Email))
                    .ReturnsAsync((ApplicationUser)null);

                // Act
                Func<Task<User>> action = () => ServiceUnderTest.GetUserByEmailAsync(internalUser.Email);

                // Assert
                action.ShouldThrow<UserNotFoundException>();
            }
        }

        public class CreateUserWithPasswordAsync : UserServiceTest
        {
            [Fact]
            public async Task Should_create_user_and_return_successful_result()
            {
                // Arrange
                var user = Fixture.Create<User>();
                var password = Fixture.Create<string>();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.CreateUserWithPasswordAsync(It.IsAny<ApplicationUser>(), password))
                    .ReturnsAsync(true);

                // Act
                var result = await ServiceUnderTest.CreateUserWithPasswordAsync(user, password);

                // Assert
                result.Succeeded.Should().BeTrue();
            }

            [Fact]
            public async Task Should_return_failed_result_when_user_can_not_be_created()
            {
                // Arrange
                var user = Fixture.Create<User>();
                var password = Fixture.Create<string>();

                Mock.Mock<IUserRepository>()
                    .Setup(repository => repository.CreateUserWithPasswordAsync(It.IsAny<ApplicationUser>(), password))
                    .ReturnsAsync(false);

                // Act
                var result = await ServiceUnderTest.CreateUserWithPasswordAsync(user, password);

                // Assert
                result.Succeeded.Should().BeFalse();
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