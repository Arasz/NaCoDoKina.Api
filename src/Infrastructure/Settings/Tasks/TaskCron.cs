using NCrontab;
using System;

namespace Infrastructure.Settings.Tasks
{
    public class TaskCron
    {
        public string Cron { get; set; }

        public string Description { get; set; }

        public DateTime NextOccurrence() => CrontabSchedule
            .Parse(Cron)
            .GetNextOccurrence(DateTime.Now);
    }
}