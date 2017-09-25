﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.IntegrationTests.Api.Extensions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class AuthControllerTest : HttpTestWithDatabase
    {
        public class GetUserToken : AuthControllerTest
        {
            [Fact]
            public async Task Should_return_token_when_registered_user_tries_to_log_in()
            {
                // Arrange
                var user = await GetDbContext<ApplicationIdentityContext>().Users.FirstAsync();
                var url = $"{ApiSettings.Version}/auth/token";
                var payload = new Credentials
                {
                    UserName = user.UserName,
                    Password = ApiSettings.DefaultUserPassword
                };
                var tokenHandler = new JwtSecurityTokenHandler();

                // Act
                var response = await Client.PostAsync(url, GetPayload(payload));

                // Assert
                response.EnsureSuccessStatusCode();

                var token = await response.Content.ReadAsJsonObjectAsync<JwtToken>();
                token.Token.Should().NotBeNullOrEmpty();

                var parsedToken = tokenHandler.ReadJwtToken(token.Token);
                parsedToken.Claims.Should()
                    .ContainSingle(claim => claim.Type == JwtRegisteredClaimNames.UniqueName);

                token.Expires.Should().BeAfter(DateTime.Now);
            }
        }
    }
}