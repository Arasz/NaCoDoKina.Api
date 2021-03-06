﻿using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Services;
using Ploeh.AutoFixture;
using System;
using System.Threading.Tasks;
using ApplicationCore.Results;
using Infrastructure.Models.Users;
using Infrastructure.Services;
using TestsCore;
using Xunit;

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
                .Setup(mapper => mapper.Map<Credentials>(It.IsAny<User>()))
                .Returns(new Func<User, Credentials>(credentials => new Credentials
                {
                    UserName = credentials.UserName,
                }));
            Mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<User>(It.IsAny<RegisterUser>()))
                .Returns(new Func<RegisterUser, User>(user => new User
                {
                    UserName = user.UserName,
                    Email = user.Email
                }));
        }

        public class CreateUser : UsersControllerTest
        {
            [Fact]
            public async Task Should_return_CreatedAtAction_result_when_user_created()
            {
                // Arrange
                var registerUser = Fixture.Create<RegisterUser>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.CreateUserWithPasswordAsync(It.IsAny<User>(), registerUser.Password))
                    .ReturnsAsync(new Func<User, string, Result<User>>((user, pswd) => Result<User>.CreateSucceeded(user)));

                // Act
                var result = await ControllerUnderTest.CreateUser(registerUser);

                // Assert
                result.Should().BeOfType<CreatedResult>();
                var body = (result as CreatedResult)?.Value as Credentials;
                body.Should().NotBeNull();
                body.Password.Should().BeNull();
                body.UserName.Should().Be(registerUser.UserName);
            }

            [Fact]
            public async Task Should_return_InternalServerError_when_user_can_not_be_created()
            {
                // Arrange
                var registerUser = Fixture.Create<RegisterUser>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.CreateUserWithPasswordAsync(It.IsAny<User>(), registerUser.Password))
                    .ReturnsAsync(Result<User>.Failure<User>());

                // Act
                var result = await ControllerUnderTest.CreateUser(registerUser);

                // Assert
                result.Should().BeOfType<ObjectResult>();
                (result as ObjectResult)?
                    .StatusCode.Should().Be(500);
            }
        }
    }
}