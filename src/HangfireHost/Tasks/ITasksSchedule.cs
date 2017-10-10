namespace HangfireHost.Tasks
{
    public interface ITasksSchedule
    {
        /// <summary>
        /// Schedule tasks to execution by task engine 
        /// </summary>
        void Schedule();
    }
}