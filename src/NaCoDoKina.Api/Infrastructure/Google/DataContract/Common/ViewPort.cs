namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Common
{
    /// <summary>
    /// Contains the recommended viewport for displaying the returned result. Generally the viewport
    /// is used to frame a result when displaying it to a user.
    /// </summary>
    public class ViewPort
    {
        /// <summary>
        /// Northeast corner of the viewport bounding box 
        /// </summary>
        public Location Northeast { get; set; }

        /// <summary>
        /// Southwest corner of the viewport bounding box 
        /// </summary>
        public Location Southwest { get; set; }
    }
}