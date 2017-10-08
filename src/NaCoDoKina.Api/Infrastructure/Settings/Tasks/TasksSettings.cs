namespace NaCoDoKina.Api.Infrastructure.Settings.Tasks
{
    public class TasksSettings
    {
        public TaskCron CinemaNetworksTask { get; set; }

        public TaskCron CinemasTask { get; set; }

        public TaskCron MoviesTask { get; set; }

        public TaskCron ShowtimesTask { get; set; }
    }
}