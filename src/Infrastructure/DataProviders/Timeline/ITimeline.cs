using System;

namespace Infrastructure.DataProviders.Timeline
{
    /// <summary>
    /// Represents time line on which we can move 
    /// </summary>
    public interface ITimeline
    {
        DateTime Position { get; }

        TimeSpan DefaultStep { get; }

        /// <summary>
        /// Moves on timeline by default step 
        /// </summary>
        void Move();

        /// <summary>
        /// Moves by given step on timeline 
        /// </summary>
        /// <param name="step"> Timeline step </param>
        void Move(TimeSpan step);

        /// <summary>
        /// Moves to given position on timeline 
        /// </summary>
        /// <param name="position"> Position on timeline </param>
        void Move(DateTime position);
    }
}