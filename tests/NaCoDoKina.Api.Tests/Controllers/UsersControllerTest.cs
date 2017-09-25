using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Results;
using NaCoDoKina.Api.Services;
using Ploeh.AutoFixture;
using System;
using System.Threading.Tasks;
using Xunit;
using JwtToken = NaCoDoKina.Api.DataContracts.Authentication.JwtToken;

namespace NaCoDoKina.Api.Controllers
{
    public class UsersControllerTest : UnitTestBase
    {
        protected UsersController ControllerUnderTest { get; }

        public UsersControllerTest()
        {
            ControllerUnderTest = Mock.Create<UsersController>();
            SetupMapper();
        }

        private void SetupMapper()
        {
            Mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<User>(It.IsAny<Credentials>()))
                .Returns(new Func<Credentials, User>(credentials => new User
                {
                    UserName = credentials.UserName,
                }));
            Mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<JwtToken>(It.IsAny<AuthToken>()))
                .Returns(new Func<AuthToken, JwtToken>(authToken => new JwtToken
                {
                    Token = authToken.Token,
                    Expires = authToken.Expires
                }));
        }

        public class GetTokenForUser : UsersControllerTest
        {
            [Fact]
            public async Task Should_return_OkObjectResult_with_token_when_credentials_are_correct()
            {
                // Arrange
                var token = Fixture.Create<AuthToken>();
                var credentials = Fixture.Create<Credentials>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.AuthenticateAsync(It.IsAny<User>(), credentials.Password))
                    .ReturnsAsync(Result.CreateSucceeded);

                Mock.Mock<ITokenService>()
                    .Setup(service => service.CreateToken(It.IsAny<User>()))
                    .Returns(token);

                // Act
                var actionResult = await ControllerUnderTest.GetTokenForUser(credentials);

                // Assert
                actionResult.Should().BeOfType<OkObjectResult>();
                var tokenFromResult = (actionResult as OkObjectResult)?.Value as JwtToken;
                tokenFromResult.Should().NotBeNull();
                tokenFromResult.Token.Should().Be(token.Token);
            }

            [Fact]
            public async Task Should_return_UnauthorizedResult_when_credentials_are_incorrect()
            {
                // Arrange
                var token = Fixture.Create<AuthToken>();
                var credentials = Fixture.Create<Credentials>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.AuthenticateAsync(It.IsAny<User>(), credentials.Password))
                    .ReturnsAsync(Result.CreateFailed());

                Mock.Mock<ITokenService>()
                    .Setup(service => service.CreateToken(It.IsAny<User>()))
                    .Returns(token);

                // Act
                var actionResult = await ControllerUnderTest.GetTokenForUser(credentials);

                // Assert
                actionResult.Should().BeOfType<UnauthorizedResult>();
            }

            [Fact]
            public async Task Should_return_BadRequestResult_when_credentials_are_in_incorrect_state()
            {
                // Arrange
                var token = Fixture.Create<AuthToken>();
                var credentials = Fixture.Build<Credentials>()
                    .OmitAutoProperties()
                    .Create();

                Mock.Mock<IUserService>()
                    .Setup(service => service.AuthenticateAsync(It.IsAny<User>(), credentials.Password))
                    .ReturnsAsync(Result.CreateFailed());

                Mock.Mock<ITokenService>()
                    .Setup(service => service.CreateToken(It.IsAny<User>()))
                    .Returns(token);

                // Act
                var actionResult = await ControllerUnderTest.GetTokenForUser(credentials);

                // Assert
                actionResult.Should().BeOfType<BadRequestResult>();
            }
        }
    }
}