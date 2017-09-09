namespace NaCoDoKina.Api.Repository
{
    public abstract class RepositoryTestBase<TRepository>
    {
        protected TRepository RepositoryUnderTest { get; set; }
    }
}