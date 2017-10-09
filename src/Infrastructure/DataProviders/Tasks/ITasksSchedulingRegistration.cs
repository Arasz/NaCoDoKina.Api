namespace Infrastructure.DataProviders.Tasks
{
    /// <summary>
    /// Registers tasks with concrete schedule 
    /// </summary>
    public interface ITasksSchedulingRegistration
    {
        /// <summary>
        /// Registers tasks 
        /// </summary>
        void Register();
    }
}