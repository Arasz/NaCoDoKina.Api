namespace NaCoDoKina.Api.DataContracts.Accounts
{
    /// <summary>
    /// Minimum amount of user data needed to registration 
    /// </summary>
    public class RegisterUser
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}