namespace NaCoDoKina.Api.Infrastructure.Settings.Tasks
{
    public class TasksSettings
    {
        public TaskCron CinemaNetworkTask { get; set; }

        public TaskCron CinemaTask { get; set; }

        public TaskCron MovieTask { get; set; }

        public TaskCron MovieShowtimeTask { get; set; }
    }
}