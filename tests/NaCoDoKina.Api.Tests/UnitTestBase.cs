using Ploeh.AutoFixture;

namespace NaCoDoKina.Api
{
    /// <summary>
    /// Identifies unit test and provides common tools and dependencies 
    /// </summary>
    public abstract class UnitTestBase
    {
        protected Fixture Fixture { get; }

        protected UnitTestBase()
        {
            Fixture = new Fixture();
        }
    }
}