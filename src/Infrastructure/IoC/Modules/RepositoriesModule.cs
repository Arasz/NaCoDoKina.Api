namespace Infrastructure.IoC.Modules
{
    public class RepositoriesModule : NamingConventionModule
    {
        protected override string ConventionSuffix => "Repository";
    }
}