namespace NaCoDoKina.Api.DataContracts.Authentication
{
    /// <summary>
    /// Minimum amount of user data needed to registration 
    /// </summary>
    public class CreateUser
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}