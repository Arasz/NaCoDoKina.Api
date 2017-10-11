using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Services.Identity;
using TestsCore;
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
            public void Should_return_correct_id_when_claim_exist_and_has_correct_value()
            {
                // Arrange
                var id = Fixture.Create<long>();

                var principal = Fixture.Build<ClaimsPrincipal>()
                    .Create();
                principal.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, id.ToString())
                }));

                var context = new DefaultHttpContext { User = principal };

                Mock.Mock<IHttpContextAccessor>()
                    .Setup(accessor => accessor.HttpContext)
                    .Returns(context);

                // Act
                var result = ServiceUnderTest.Id;

                // Assert
                result.Should().Be(id);
            }

            [Fact]
            public void Should_return_0_when_claim_do_not_exist()
            {
                // Arrange
                var id = Fixture.Create<long>();

                var principal = Fixture.Build<ClaimsPrincipal>()
                    .Create();

                var context = new DefaultHttpContext { User = principal };

                Mock.Mock<IHttpContextAccessor>()
                    .Setup(accessor => accessor.HttpContext)
                    .Returns(context);

                // Act
                var result = ServiceUnderTest.Id;

                // Assert
                result.Should().Be(0);
            }

            [Fact]
            public void Should_return_0_when_claim_has_incorrect_value()
            {
                // Arrange
                var incorrectId = Fixture.Create<string>();

                var principal = Fixture.Build<ClaimsPrincipal>()
                    .Create();
                principal.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, incorrectId)
                }));

                var context = new DefaultHttpContext { User = principal };

                Mock.Mock<IHttpContextAccessor>()
                    .Setup(accessor => accessor.HttpContext)
                    .Returns(context);

                // Act
                var result = ServiceUnderTest.Id;

                // Assert
                result.Should().Be(0);
            }
        }
    }
}