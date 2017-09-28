namespace NaCoDoKina.Api.DataContracts.Authentication
{
    /// <summary>
    /// Credentials required to authentication 
    /// </summary>
    public class Credentials
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public override string ToString()
        {
            return $"{nameof(UserName)}: {UserName}, {nameof(Password)}: {Password}";
        }
    }
}