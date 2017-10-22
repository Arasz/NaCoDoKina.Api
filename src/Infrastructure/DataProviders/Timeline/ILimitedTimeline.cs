namespace Infrastructure.DataProviders.Timeline
{
    /// <summary>
    /// Represents timeline with limited values 
    /// </summary>
    public interface ILimitedTimeline : ITimeline
    {
        /// <summary>
        /// Checks if current position is within timeline range 
        /// </summary>
        /// <returns></returns>
        bool IsInRange();

        /// <summary>
        /// Range that is limiting this timeline 
        /// </summary>
        TimelineRange TimelineRange { get; }
    }
}