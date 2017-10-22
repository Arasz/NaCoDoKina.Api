using System.Threading.Tasks;

namespace Infrastructure.DataProviders.Tasks
{
    /// <summary>
    /// Task executed in the background job 
    /// </summary>
    public interface ITask
    {
        string Id { get; }

        /// <summary>
        /// Executes task 
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();
    }
}