using FluentAssertions;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.IntegrationTests.Api.Extensions;
using Ploeh.AutoFixture;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class UsersControllerTest : HttpTestWithDatabase
    {
        public class CreateUser : UsersControllerTest
        {
            [Fact]
            public async Task Should_create_new_user_and_login()
            {
                // Arrange
                var registerUrl = $"{ApiSettings.Version}/users";
                var loginUrl = $"{ApiSettings.Version}/auth/token";
                var email = "test@kmail.com";

                var registerPayload = new RegisterUser
                {
                    UserName = Fixture.Create<string>(),
                    Email = email,
                    Password = ApiSettings.DefaultUserPassword,
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                // Act
                var registerResponse = await Client.PostAsync(registerUrl, GetPayload(registerPayload));

                // Assert

                registerResponse.EnsureSuccessStatusCode();

                var credentials = await registerResponse.Content.ReadAsJsonObjectAsync<Credentials>();
                credentials.Password = ApiSettings.DefaultUserPassword;

                var loginResponse = await Client.PostAsync(loginUrl, GetPayload(credentials));

                loginResponse.EnsureSuccessStatusCode();

                var token = await loginResponse.Content.ReadAsJsonObjectAsync<JwtToken>();
                token.Token.Should().NotBeNullOrEmpty();

                var parsedToken = tokenHandler.ReadJwtToken(token.Token);
                parsedToken.Claims.Should()
                    .ContainSingle(claim => claim.Type == JwtRegisteredClaimNames.UniqueName);

                token.Expires.Should().BeAfter(DateTime.Now);
            }
        }
    }
}