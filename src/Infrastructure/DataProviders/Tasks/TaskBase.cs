using Infrastructure.Settings.Tasks;
using System.Threading.Tasks;

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
            Id = GetType().FullName;
        }

        public string Id { get; }

        public abstract Task Execute();
    }
}