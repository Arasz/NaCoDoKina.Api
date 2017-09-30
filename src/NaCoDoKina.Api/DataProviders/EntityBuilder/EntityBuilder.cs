using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder
{
    public class EntityBuilder<TEntity> : IEntityBuilder<TEntity>
        where TEntity : Entities.Entity, new()
    {
        public bool Successful { get; private set; }

        public BuildFailure BuildFailure { get; private set; }

        public IReadOnlyList<IBuildStep<TEntity>> BuildSteps { get; }

        public int CurrentStep { get; private set; }

        public EntityBuilder(IEnumerable<IBuildStep<TEntity>> buildSteps)
        {
            BuildSteps = buildSteps.ToImmutableList();
        }

        public async Task<TEntity> Build()
        {
            var entity = new TEntity();

            foreach (var buildStep in BuildSteps.OrderBy(step => step.Position))
            {
                CurrentStep++;

                var result = await buildStep.Build(entity);

                if (result.IsSuccess)
                    entity = result.Value;
                else
                {
                    Successful = false;
                    BuildFailure = new BuildFailure(result.FailureReason, CurrentStep);
                    return entity;
                }
            }

            Successful = true;
            return entity;
        }
    }
}