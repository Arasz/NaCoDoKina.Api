using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure.Identity
{
    public class JwtClaimAuthenticatedUserIdTest : UnitTestBase
    {
        private IAuthenticatedUserId ServiceUnderTest { get; }

        public JwtClaimAuthenticatedUserIdTest()
        {
            ServiceUnderTest = Mock.Create<JwtClaimAuthenticatedUserId>();
        }

        public class Id : JwtClaimAuthenticatedUserIdTest
        {
            [Fact]
            public void Should_return_correct_id()
            {
                // Arrange
                var id = Fixture.Create<long>();

                var principal = Fixture.Build<ClaimsPrincipal>()
                    .Create();
                principal.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, id.ToString())
                }));

                var context = new DefaultHttpContext();
                context.User = principal;

                Mock.Mock<IHttpContextAccessor>()
                    .Setup(accessor => accessor.HttpContext)
                    .Returns(context);

                // Act
                var result = ServiceUnderTest.Id;

                // Assert
                result.Should().Be(id);
            }
        }
    }
}