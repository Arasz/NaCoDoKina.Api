using Infrastructure.Settings.Tasks;

namespace Infrastructure.DataProviders.Tasks
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