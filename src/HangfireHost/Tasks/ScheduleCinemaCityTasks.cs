using Hangfire;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Infrastructure.DataProviders.CinemaCity.Movies.Tasks;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Tasks;
using Infrastructure.DataProviders.Common.CinemaNetworks;
using Infrastructure.Settings.Tasks;
using System;

namespace HangfireHost.Tasks
{
    public class ScheduleCinemaCityTasks : ITasksSchedule
    {
        private readonly TasksSettings _settings;

        public ScheduleCinemaCityTasks(TasksSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public string Name => "Schedule tasks";

        public void Schedule()
        {
            RecurringJob
                .AddOrUpdate<LoadCinemaNetworksFromConfigurationTask>(task => task.ExecuteAsync(), () => _settings.CinemaNetworksTask.Cron);

            RecurringJob
                .AddOrUpdate<LoadCinemaCityCinemasTask>(task => task.ExecuteAsync(), () => _settings.CinemasTask.Cron);

            RecurringJob
                .AddOrUpdate<LoadCinemaCityMoviesTask>(task => task.ExecuteAsync(), () => _settings.MoviesTask.Cron);

            RecurringJob
                .AddOrUpdate<LoadCinemaCityShowtimesTask>(task => task.ExecuteAsync(), () => _settings.ShowtimesTask.Cron);
        }
    }
}