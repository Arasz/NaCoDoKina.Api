namespace NaCoDoKina.Api.Infrastructure.Settings
{
    public class ConnectionStringsSettings
    {
        public string DataConnection { get; set; }

        public string IdentityConnection { get; set; }

        public string AppendDatabasePassword(string connection, string password)
        {
            return $"{connection}{password}";
        }
    }
}