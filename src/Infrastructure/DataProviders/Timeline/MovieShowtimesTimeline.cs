using Infrastructure.Settings.Tasks;
using System;

namespace Infrastructure.DataProviders.Timeline
{
    public class MovieShowtimesTimeline : LimitedTimeline
    {
        public MovieShowtimesTimeline(TasksSettings tasksSettings) :
            base(DateTime.Today, tasksSettings.ShowtimesTask.NextOccurrence().AddDays(1), TimeSpan.FromDays(1))
        {
        }
    }
}