using Infrastructure.Settings.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.Tasks
{
    /// <summary>
    /// Task that executes periodically with interval described by cron expression 
    /// </summary>
    public abstract class TaskBase : ITask
    {
        protected TasksSettings Settings { get; }

        protected ILogger<TaskBase> Logger { get; }

        protected TaskBase(TasksSettings settings, ILogger<TaskBase> logger)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Id = GetType().FullName;
        }

        public string Id { get; }

        public abstract Task ExecuteAsync();
    }
}