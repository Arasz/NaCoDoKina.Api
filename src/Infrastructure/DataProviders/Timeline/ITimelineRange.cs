using System;

namespace Infrastructure.DataProviders.Timeline
{
    public interface ITimelineRange
    {
        DateTime Start { get; }
        DateTime End { get; }
        bool IsInRange(DateTime dateTime);
    }
}