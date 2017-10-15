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
                .Enqueue<LoadCinemaNetworksFromConfigurationTask>(task => task.Execute());

            var cinemasTaskId = BackgroundJob
                .ContinueWith<LoadCinemaCityCinemasTask>(cinemaNetworksTaskId, task => task.Execute());

            var moviesTaskId = BackgroundJob
                .ContinueWith<LoadCinemaCityMoviesTask>(cinemasTaskId, task => task.Execute());

            BackgroundJob
                .ContinueWith<CinemaCityShowtimesTask>(moviesTaskId, task => task.Execute());
        }
    }
}