using System;

namespace Infrastructure.DataProviders.Timeline
{
    public class TimelineRange : ITimelineRange
    {
        public TimelineRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public bool IsInRange(DateTime dateTime)
        {
            return dateTime >= Start && dateTime <= End;
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
        }
    }
}