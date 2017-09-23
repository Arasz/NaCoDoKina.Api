using Autofac;

namespace NaCoDoKina.Api
{
    public abstract class UnitTestBase
    {
        protected IContainer Container { get; set; }

        protected abstract void BuildContainer();
    }
}