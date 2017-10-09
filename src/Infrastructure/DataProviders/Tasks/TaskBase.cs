using System.Threading.Tasks;
using Infrastructure.Settings.Tasks;

namespace Infrastructure.DataProviders.Tasks
{
    /// <summary>
    /// Task that executes periodically with interval described by cron expression 
    /// </summary>
    public abstract class TaskBase : ITask
    {
        protected TasksSettings Settings { get; }

        protected TaskBase(TasksSettings settings)
        {
            Settings = settings;
        }

        public abstract Task Execute();
    }
}