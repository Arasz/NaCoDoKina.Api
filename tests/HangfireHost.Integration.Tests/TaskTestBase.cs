using Infrastructure.DataProviders.Tasks;
using IntegrationTestsCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HangfireHost.Integration.Tests
{
    [Collection("Tests with hangfire")]
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