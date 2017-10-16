namespace Infrastructure.Exceptions
{
    public class UserNotFoundException : NaCoDoKinaApiException
    {
        public UserNotFoundException() : base("There is no registered user")
        {
        }

        public UserNotFoundException(string userName)
            : base($"RegisterUser {userName} was not found")
        {
        }
    }
}