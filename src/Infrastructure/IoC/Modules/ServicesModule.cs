namespace Infrastructure.IoC.Modules
{
    public class ServicesModule : NamingConventionModule
    {
        protected override string ConventionSuffix => "Service";
    }
}