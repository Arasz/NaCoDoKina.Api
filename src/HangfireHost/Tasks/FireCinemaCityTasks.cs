using Hangfire;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Infrastructure.DataProviders.CinemaCity.Movies.Tasks;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Tasks;
using Infrastructure.DataProviders.Common.CinemaNetworks;

namespace HangfireHost.Tasks
{
    public class FireCinemaCityTasks : ITasksSchedule
    {
        public string Name => "Fire tasks once";

        public void Schedule()
        {
            var cinemaNetworksTaskId = BackgroundJob
                .Enqueue<LoadCinemaNetworksFromConfigurationTask>(task => task.ExecuteAsync());

            var cinemasTaskId = BackgroundJob
                .ContinueWith<LoadCinemaCityCinemasTask>(cinemaNetworksTaskId, task => task.ExecuteAsync());

            var moviesTaskId = BackgroundJob
                .ContinueWith<LoadCinemaCityMoviesTask>(cinemasTaskId, task => task.ExecuteAsync());

            BackgroundJob
                .ContinueWith<LoadCinemaCityShowtimesTask>(moviesTaskId, task => task.ExecuteAsync());
        }
    }
}