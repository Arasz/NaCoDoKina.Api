using System.ComponentModel.DataAnnotations;

namespace NaCoDoKina.Api.DataContracts.Authentication
{
    /// <summary>
    /// Minimum amount of user data needed to registration 
    /// </summary>
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }

        [EmailAddress, Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}