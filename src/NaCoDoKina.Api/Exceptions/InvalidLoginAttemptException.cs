namespace NaCoDoKina.Api.Exceptions
{
    public class InvalidLoginAttemptException : NaCoDoKinaApiException
    {
        public InvalidLoginAttemptException()
            : base("Invalid login attempt")
        {
        }
    }
}