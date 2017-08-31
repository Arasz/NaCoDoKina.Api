using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class LocationServiceTest
    {
        protected ILocationService ServiceUnderTest { get; }

        public LocationServiceTest()
        {
        }
    }

    public class CalculateTravelTimeAsync
    {
        [Fact]
        public async Task Should_return_correct_time()
        {
        }
    }
}