using NaCoDoKina.Api.Infrastructure.Settings.Tasks;

namespace NaCoDoKina.Api.DataProviders.Tasks
{
    public class DefaultTasksRegistration : ITasksSchedulingRegistration
    {
        public DefaultTasksRegistration(TasksSettings tasksSettings)
        {
        }

        public void Register()
        {
            throw new System.NotImplementedException();
        }
    }
}