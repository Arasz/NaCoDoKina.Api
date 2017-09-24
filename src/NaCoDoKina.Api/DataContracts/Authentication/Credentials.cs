using System.ComponentModel.DataAnnotations;

namespace NaCoDoKina.Api.DataContracts.Authentication
{
    /// <summary>
    /// Credentials required to authentication 
    /// </summary>
    public class Credentials
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}