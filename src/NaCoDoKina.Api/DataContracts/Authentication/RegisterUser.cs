namespace NaCoDoKina.Api.DataContracts.Authentication
{
    /// <summary>
    /// Minimum amount of user data needed to registration 
    /// </summary>
    public class RegisterUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public override string ToString()
        {
            return $"{nameof(UserName)}: {UserName}, {nameof(Email)}: {Email}, {nameof(Password)}: {Password}";
        }
    }
}