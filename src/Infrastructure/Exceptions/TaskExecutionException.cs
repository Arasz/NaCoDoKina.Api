using Infrastructure.DataProviders.EntityBuilder;

namespace Infrastructure.Exceptions
{
    public class TaskExecutionException : NaCoDoKinaApiException
    {
        public TaskExecutionException(BuildFailure buildFailure)
            : base(buildFailure.ToString())
        {
        }
    }
}