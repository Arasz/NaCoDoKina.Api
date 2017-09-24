using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.IntegrationTests.Api.DatabaseInitializer;
using NaCoDoKina.Api.IntegrationTests.Api.Extensions;
using System;
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

                // Act
                var response = await Client.PostAsync(url, GetPayload(payload));

                // Assert
                Action ensureAction = () => response.EnsureSuccessStatusCode();
                ensureAction.ShouldNotThrow();
                var token = await response.Content.ReadAsJsonObjectAsync<JwtToken>();
                token.Token.Should().NotBeNullOrEmpty();
                token.Expires.Should().BeAfter(DateTime.Now);
            }
        }
    }
}