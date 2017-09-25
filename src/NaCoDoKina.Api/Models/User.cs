namespace NaCoDoKina.Api.Models
{
    /// <summary>
    /// Application user 
    /// </summary>
    public class User
    {
        /// <summary>
        /// User id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User name 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User email 
        /// </summary>
        public string Email { get; set; }
    }
}