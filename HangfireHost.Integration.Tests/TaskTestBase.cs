using Infrastructure.DataProviders.Tasks;
using IntegrationTestsCore;
using Microsoft.Extensions.DependencyInjection;

namespace HangfireHost.Integration.Tests
{
    public class TaskTestBase<TTask> : HttpTestWithDatabase<Startup>
        where TTask : ITask
    {
        protected ITask TaskUnderTest { get; }

        public TaskTestBase()
        {
            TaskUnderTest = Services.GetService<TTask>();
        }
    }
}