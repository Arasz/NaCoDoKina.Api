namespace Infrastructure.DataProviders.EntityBuilder
{
    public class BuildFailure
    {
        public BuildFailure(string reason, int step)
        {
            Reason = reason;
            Step = step;
        }

        public string Reason { get; }

        public int Step { get; }
    }
}