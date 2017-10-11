namespace HangfireHost.Tasks
{
    public interface ITasksSchedule
    {
        /// <summary>
        /// Schedule name 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Schedule tasks to execution by task engine 
        /// </summary>
        void Schedule();
    }
}