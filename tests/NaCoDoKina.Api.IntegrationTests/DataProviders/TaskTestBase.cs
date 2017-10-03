using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.DataProviders.Tasks;
using NaCoDoKina.Api.IntegrationTests.Api;

namespace NaCoDoKina.Api.IntegrationTests.DataProviders
{
    public class TaskTestBase<TTask> : HttpTestWithDatabase
        where TTask : ITask
    {
        protected ITask TaskUnderTest { get; }

        public TaskTestBase()
        {
            TaskUnderTest = Services.GetService<TTask>();
        }
    }
}