namespace Infrastructure.DataProviders.EntityBuilder
{
    public class BuildFailure
    {
        public BuildFailure(string reason, int step, int stepsCount, string stepName)
        {
            Reason = reason;
            Step = step;
            StepsCount = stepsCount;
            StepName = stepName;
            Description = ToString();
        }

        public string Reason { get; }

        public int Step { get; }

        public int StepsCount { get; }

        public string StepName { get; }

        public string Description { get; }

        public override string ToString()
        {
            return $"Build failed on {StepName} step, executed as {Step}/{StepsCount}, because {Reason}";
        }
    }
}