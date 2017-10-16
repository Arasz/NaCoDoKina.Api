using Autofac.Extras.Moq;
using Ploeh.AutoFixture;
using System;

namespace TestsCore
{
    /// <inheritdoc/>
    /// <summary>
    /// Identifies unit test and provides common tools and dependencies 
    /// </summary>
    public abstract class UnitTestBase : IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Mock.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected Fixture Fixture { get; }

        protected AutoMock Mock { get; }

        protected UnitTestBase()
        {
            Fixture = new Fixture();
            Mock = AutoMock.GetLoose();
        }
    }
}