using System.Threading.Tasks;

namespace Infrastructure.DataProviders.Tasks
{
    /// <summary>
    /// Task executed in the background job 
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Executes task 
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }
}