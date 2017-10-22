using System;

namespace Infrastructure.DataProviders.Timeline
{
    public class LimitedTimeline : ILimitedTimeline
    {
        public DateTime Position { get; private set; }

        public TimeSpan DefaultStep { get; }

        public LimitedTimeline(DateTime start, DateTime end, TimeSpan defaultStep)
        {
            TimelineRange = new TimelineRange(start, end);
            Position = start;
            DefaultStep = defaultStep;
        }

        public void Move()
        {
            Position = Position.Add(DefaultStep);
        }

        public void Move(TimeSpan step)
        {
            Position = Position.Add(step);
        }

        public void Move(DateTime position)
        {
            Position = position;
        }

        public bool IsInRange() => TimelineRange.IsInRange(Position);

        public TimelineRange TimelineRange { get; }

        public override string ToString()
        {
            return $"{nameof(Position)}: {Position}, {nameof(DefaultStep)}: {DefaultStep}, {nameof(TimelineRange)}: {TimelineRange}";
        }
    }
}