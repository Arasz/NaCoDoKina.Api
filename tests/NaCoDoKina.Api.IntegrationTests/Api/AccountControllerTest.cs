using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.IntegrationTests.Api.DatabaseInitializer;
using NaCoDoKina.Api.IntegrationTests.Api.Extensions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class AccountControllerTest : HttpTestBase
    {
        private IdentityDataSeed IdentityContextSeed { get; set; }

        /// <inheritdoc/>
        protected override async Task SeedDatabaseAsync()
        {
            IdentityContextSeed = Services
                .GetService<IDatabaseSeed<ApplicationIdentityContext>>() as IdentityDataSeed
                ?? throw new ArgumentNullException(nameof(IdentityContextSeed));

            await IdentityContextSeed.SeedAsync();
        }

        public class Register : AccountControllerTest
        {
            [Fact]
            public async Task Should_create_new_user_and_login()
            {
                // Arrange
                await SeedDatabaseAsync();
                var user = await IdentityContextSeed.DbContext.Users.FirstAsync();
                var registerUrl = $"{Version}/account";
                var loginUrl = $"{Version}/account/token";
                var email = "test@kmail.com";

                var registerPayload = new RegisterUser
                {
                    Email = email,
                    Password = IdentityContextSeed.UniversalPassword
                };

                var loginPayload = new LoginUser
                {
                    Email = user.Email,
                    Password = IdentityContextSeed.UniversalPassword
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                // Act
                var registerResponse = await Client.PostAsync(registerUrl, GetPayload(registerPayload));

                // Assert

                registerResponse.EnsureSuccessStatusCode();

                var loginResponse = await Client.PostAsync(loginUrl, GetPayload(loginPayload));

                loginResponse.EnsureSuccessStatusCode();

                var token = await loginResponse.Content.ReadAsJsonObjectAsync<JwtToken>();
                token.Token.Should().NotBeNullOrEmpty();

                var parsedToken = tokenHandler.ReadJwtToken(token.Token);
                parsedToken.Claims.Should()
                    .ContainSingle(claim => claim.Type == JwtRegisteredClaimNames.UniqueName);

                token.Expires.Should().BeAfter(DateTime.Now);
            }
        }

        public class Login : AccountControllerTest
        {
            [Fact]
            public async Task Should_return_token_when_registered_user_tries_to_log_in()
            {
                // Arrange
                await SeedDatabaseAsync();
                var user = await IdentityContextSeed.DbContext.Users.FirstAsync();
                var url = $"{Version}/account/token";
                var payload = new LoginUser
                {
                    Email = user.Email,
                    Password = IdentityContextSeed.UniversalPassword
                };
                var tokenHandler = new JwtSecurityTokenHandler();

                // Act
                var response = await Client.PostAsync(url, GetPayload(payload));

                // Assert
                Action ensureAction = () => response.EnsureSuccessStatusCode();
                ensureAction.ShouldNotThrow();
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