using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NaCoDoKina.Api.Infrastructure.Identity;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Repositories;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class UserRepositoryTest : RepositoryTestBase<IUserRepository, ApplicationIdentityContext>
    {
        protected UserManager<ApplicationUser> CreateUserManager(ApplicationIdentityContext dbContext,
            PasswordHasher<ApplicationUser> passwordHasher = null,
            IdentityResult userValidationResult = null,
            IdentityResult passwordValidationResult = null)
        {
            userValidationResult = userValidationResult ?? IdentityResult.Success;
            passwordValidationResult = passwordValidationResult ?? IdentityResult.Success;
            passwordHasher = passwordHasher ?? new PasswordHasher<ApplicationUser>();

            var userStore = new UserStore<ApplicationUser, ApplicationRole, ApplicationIdentityContext, long>(dbContext);

            Mock.Mock<IUserValidator<ApplicationUser>>()
                .Setup(validator => validator.ValidateAsync(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<ApplicationUser>()))
                .ReturnsAsync(() => userValidationResult);
            var userValidator = Mock.Mock<IUserValidator<ApplicationUser>>();

            Mock.Mock<IPasswordValidator<ApplicationUser>>()
                .Setup(validator => validator.ValidateAsync(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => passwordValidationResult);
            var passwordValidator = Mock.Mock<IPasswordValidator<ApplicationUser>>();

            return new UserManager<ApplicationUser>(userStore,
                Mock.Mock<IOptions<IdentityOptions>>().Object,
                passwordHasher,
                new[] { userValidator.Object },
                new[] { passwordValidator.Object },
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                Mock.Mock<IServiceProvider>().Object,
                Mock.Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        }

        public class UpdateUserPassword : UserRepositoryTest
        {
            [Fact]
            public async Task Should_change_user_password_for_new_when_different()
            {
                // Arrange
                var oldPassword = Fixture.Create<string>();
                var newPassword = Fixture.Create<string>();
                var passwordHasher = Mock.Create<PasswordHasher<ApplicationUser>>();
                var user = Fixture.Create<ApplicationUser>();
                var oldPasswordHash = passwordHasher.HashPassword(user, oldPassword);
                user.PasswordHash = oldPasswordHash;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.Add(user);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext, passwordHasher);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    await RepositoryUnderTest.UpdateUserPassword(user, oldPassword, newPassword);
                }

                // Assert
                using (var scope = CreateContextScope())
                {
                    var userManager = CreateUserManager(scope.DbContext, passwordHasher);
                    var createdUser = await userManager.Users
                        .SingleOrDefaultAsync();

                    createdUser.Should().NotBeNull();
                    createdUser.PasswordHash.Should().NotBe(oldPasswordHash);
                }
            }

            [Fact]
            public async Task Should_change_user_password_for_new_when_the_same()
            {
                // Arrange
                var oldPassword = Fixture.Create<string>();
                var newPassword = oldPassword;
                var passwordHasher = Mock.Create<PasswordHasher<ApplicationUser>>();
                var user = Fixture.Create<ApplicationUser>();
                var oldPasswordHash = passwordHasher.HashPassword(user, oldPassword);
                user.PasswordHash = oldPasswordHash;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.Add(user);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext, passwordHasher);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    await RepositoryUnderTest.UpdateUserPassword(user, oldPassword, newPassword);
                }

                // Assert
                using (var scope = CreateContextScope())
                {
                    var userManager = CreateUserManager(scope.DbContext, passwordHasher);
                    var createdUser = await userManager.Users
                        .SingleOrDefaultAsync();

                    createdUser.Should().NotBeNull();
                    createdUser.PasswordHash.Should().NotBe(oldPasswordHash);
                }
            }
        }

        public class IncrementAccessFailed : UserRepositoryTest
        {
            [Fact]
            public async Task Should_increment_user_access_failed_counter()
            {
                // Arrange
                var initialFailCount = 0;
                var user = Fixture.Build<ApplicationUser>()
                    .With(applicationUser => applicationUser.AccessFailedCount, initialFailCount)
                    .Create();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.Add(user);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.IncrementAccessFailed(user);

                    // Assert
                    result.Should().BeTrue();
                }
                using (var scope = CreateContextScope())
                {
                    var createdUser = await scope.DbContext.Users
                        .SingleOrDefaultAsync();

                    createdUser.Should().NotBeNull();
                    createdUser.AccessFailedCount.Should().BeGreaterThan(initialFailCount);
                }
            }
        }

        public class GetAllUsersAsync : UserRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_created_users()
            {
                // Arrange
                var initialFailCount = 0;
                var users = Fixture.Build<ApplicationUser>()
                    .Without(applicationUser => applicationUser.Id)
                    .With(applicationUser => applicationUser.AccessFailedCount, initialFailCount)
                    .CreateMany(20)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.GetAllUsersAsync();

                    // Assert
                    result.Should().HaveSameCount(users);
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users
                        .Should().HaveSameCount(users);
                }
            }
        }

        public class GetUserByIdAsync : UserRepositoryTest
        {
            [Fact]
            public async Task Should_return_user_with_given_id_when_user_found()
            {
                // Arrange
                var foundUser = Fixture.Create<ApplicationUser>();
                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(20)
                    .Append(foundUser)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.GetUserByIdAsync(foundUser.Id);

                    // Assert
                    result.Id.Should().Be(foundUser.Id);
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users
                        .SingleOrDefaultAsync(user => user.Id == foundUser.Id)
                        .Should().NotBeNull();
                }
            }

            [Fact]
            public async Task Should_return_null_when_user_not_found()
            {
                // Arrange
                var foundUser = Fixture.Create<ApplicationUser>();
                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(20)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.GetUserByIdAsync(foundUser.Id);

                    // Assert
                    result.Should().BeNull();
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users
                        .Any(user => user.Id == foundUser.Id)
                        .Should().BeFalse();
                }
            }
        }

        protected void SetNormalizedPropertiesForUser(ApplicationUser user)
        {
            user.NormalizedUserName = user.UserName.Normalize();
            user.NormalizedEmail = user.Email.Normalize();
        }

        public class GetUserByNameAsync : UserRepositoryTest
        {
            [Fact]
            public async Task Should_return_user_with_given_name_when_user_found()
            {
                // Arrange
                var foundUser = Fixture.Create<ApplicationUser>();
                SetNormalizedPropertiesForUser(foundUser);
                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(5)
                    .Select(user =>
                    {
                        SetNormalizedPropertiesForUser(user);
                        return user;
                    })
                    .Append(foundUser)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.GetUserByNameAsync(foundUser.UserName);

                    // Assert
                    result.UserName.Should().Be(foundUser.UserName);
                    result.NormalizedUserName.Should().Be(foundUser.NormalizedUserName);
                    result.Id.Should().Be(foundUser.Id);
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users
                        .SingleOrDefault(user => user.Id == foundUser.Id)
                        .Should().NotBeNull();

                    scope.DbContext.Users
                        .SingleOrDefault(user => user.UserName == foundUser.UserName)
                        .Should().NotBeNull();
                }
            }

            [Fact]
            public async Task Should_return_null_when_user_not_found()
            {
                // Arrange
                var foundUser = Fixture.Create<ApplicationUser>();
                SetNormalizedPropertiesForUser(foundUser);
                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(5)
                    .Select(user =>
                    {
                        SetNormalizedPropertiesForUser(user);
                        return user;
                    })
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.GetUserByNameAsync(foundUser.UserName);

                    // Assert
                    result.Should().BeNull();
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users
                        .SingleOrDefault(user => user.Id == foundUser.Id)
                        .Should().BeNull();

                    scope.DbContext.Users
                        .SingleOrDefault(user => user.UserName == foundUser.UserName)
                        .Should().BeNull();
                }
            }
        }

        public class GetUserByEmailAsync : UserRepositoryTest
        {
            [Fact]
            public async Task Should_return_user_with_given_email_when_user_found()
            {
                // Arrange
                var foundUser = Fixture.Create<ApplicationUser>();
                SetNormalizedPropertiesForUser(foundUser);
                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(5)
                    .Select(user =>
                    {
                        SetNormalizedPropertiesForUser(user);
                        return user;
                    })
                    .Append(foundUser)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    manager.Users.Any(user => user.Email == foundUser.Email).Should().BeTrue();

                    var result = await RepositoryUnderTest.GetUserByEmailAsync(foundUser.Email);

                    // Assert
                    result.Email.Should().Be(foundUser.Email);
                    result.Id.Should().Be(foundUser.Id);
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users
                        .SingleOrDefault(user => user.Id == foundUser.Id)
                        .Should().NotBeNull();

                    scope.DbContext.Users
                        .SingleOrDefault(user => user.Email == foundUser.Email)
                        .Should().NotBeNull();
                }
            }

            [Fact]
            public async Task Should_return_null_when_user_not_found()
            {
                // Arrange
                var foundUser = Fixture.Create<ApplicationUser>();
                SetNormalizedPropertiesForUser(foundUser);
                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(5)
                    .Select(user =>
                    {
                        SetNormalizedPropertiesForUser(user);
                        return user;
                    })
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.GetUserByEmailAsync(foundUser.Email);

                    // Assert
                    result.Should().BeNull();
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users
                        .SingleOrDefault(user => user.Id == foundUser.Id)
                        .Should().BeNull();

                    scope.DbContext.Users
                        .SingleOrDefault(user => user.Email == foundUser.Email)
                        .Should().BeNull();
                }
            }
        }

        public class CreateUserWithPasswordAsync : UserRepositoryTest
        {
            [Fact]
            public async Task Should_create_user_and_return_true_when_password_and_user_data_are_correct()
            {
                // Arrange
                var password = Fixture.Create<string>();
                var existingUserCount = Fixture.Create(5);
                var newUser = Fixture.Build<ApplicationUser>()
                    .Without(user => user.Id)
                    .Without(user => user.PasswordHash)
                    .Create();

                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(existingUserCount)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext);
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.CreateUserWithPasswordAsync(newUser, password);

                    // Assert
                    result.Should().NotBeNull();
                    result.UserName.Should().Be(newUser.UserName);
                    result.Id.Should().BePositive();
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.Should().HaveCount(existingUserCount + 1);

                    scope.DbContext.Users
                        .SingleOrDefault(user => user.UserName == newUser.UserName)
                        .Should().NotBeNull();
                }
            }

            [Fact]
            public async Task Should_return_false_and_do_not_crate_new_user_when_password_is_wrong()
            {
                // Arrange
                var password = Fixture.Create("a");
                var existingUserCount = Fixture.Create(5);
                var newUser = Fixture.Build<ApplicationUser>()
                    .Without(user => user.Id)
                    .Without(user => user.PasswordHash)
                    .Create();

                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(existingUserCount)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext, passwordValidationResult: IdentityResult.Failed(new IdentityError()));
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.CreateUserWithPasswordAsync(newUser, password);

                    // Assert

                    result.Should().BeNull();
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.Should().HaveCount(existingUserCount);

                    scope.DbContext.Users
                        .SingleOrDefault(user => user.UserName == newUser.UserName)
                        .Should().BeNull();
                }
            }

            [Fact]
            public async Task Should_return_false_and_do_not_crate_new_user_when_user_data_is_wrong()
            {
                // Arrange
                var password = Fixture.Create<string>();
                var existingUserCount = Fixture.Create(5);
                var newUser = Fixture.Build<ApplicationUser>()
                    .OmitAutoProperties()
                    .With(user => user.UserName, "W")
                    .Create();

                var users = Fixture.Build<ApplicationUser>()
                    .CreateMany(existingUserCount)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.AddRange(users);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act
                using (var scope = CreateContextScope())
                {
                    var manager = CreateUserManager(scope.DbContext, userValidationResult: IdentityResult.Failed(new IdentityError()));
                    RepositoryUnderTest = new UserRepository(manager, LoggerMock.Object);

                    var result = await RepositoryUnderTest.CreateUserWithPasswordAsync(newUser, password);

                    // Assert
                    result.Should().BeNull();
                }
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Users.Should().HaveCount(existingUserCount);

                    scope.DbContext.Users
                        .SingleOrDefault(user => user.UserName == newUser.UserName)
                        .Should().BeNull();
                }
            }
        }
    }
}