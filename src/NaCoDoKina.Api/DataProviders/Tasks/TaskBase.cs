using NaCoDoKina.Api.Infrastructure.Settings;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Tasks
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