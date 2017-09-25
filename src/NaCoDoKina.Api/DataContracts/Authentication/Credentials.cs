using System.ComponentModel.DataAnnotations;

namespace NaCoDoKina.Api.DataContracts.Authentication
{
    /// <summary>
    /// Credentials required to authentication 
    /// </summary>
    public class Credentials
    {
        [Required, StringLength(40, MinimumLength = 1)]
        public string UserName { get; set; }

        [Required, StringLength(40, MinimumLength = 10)]
        public string Password { get; set; }
    }
}