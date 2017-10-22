using ApplicationCore.Entities.Movies;
using ApplicationCore.Results;
using FluentAssertions;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestsCore;
using Xunit;

namespace NaCoDoKina.Api.DataProviders.EntitiesBuilder
{
    public class EntitiesBuilderTest : UnitTestBase
    {
        private IEntitiesBuilder<Movie, EmptyContext> EntitiesBuilder { get; set; }

        public EntitiesBuilderTest()
        {
            Mock.Mock<IBuildStep<Movie, EmptyContext>>()
                .SetupGet(step => step.Enabled)
                .Returns(true);
        }

        public class BuildMany : EntitiesBuilderTest
        {
            [Fact]
            public async Task Should_return_built_entities_and_have_success_status_if_all_build_steps_succeeded()
            {
                // Arrange
                var builtMovies = Fixture.Build<Movie>()
                    .CreateMany(5)
                    .ToArray();

                Mock.Mock<IBuildStep<Movie, EmptyContext>>()
                    .Setup(step => step.BuildManyAsync(It.IsAny<Movie[]>(), It.IsAny<EmptyContext>()))
                    .ReturnsAsync(Result<Movie[]>.CreateSucceeded(builtMovies));

                // Act
                EntitiesBuilder = Mock.Create<EntitiesBuilder<Movie, EmptyContext>>();
                var entities = await EntitiesBuilder.BuildMany();

                // Assert
                EntitiesBuilder.Successful
                    .Should()
                    .BeTrue();
                entities
                    .Should()
                    .HaveSameCount(builtMovies)
                    .And
                    .Contain(builtMovies);
            }

            [Fact]
            public async Task Should_return_failure_reason_and_entities_from_last_step_if_some_build_steps_failed()
            {
                // Arrange
                var builtMovies = Fixture.Build<Movie>()
                    .CreateMany(5)
                    .ToArray();

                var success = CreateBuildStepMock(Result<Movie[]>.CreateSucceeded(builtMovies), 1);

                var failure = CreateBuildStepMock(Result<Movie[]>.CreateFailed("Failed"), 2);

                Mock.Provide<IEnumerable<IBuildStep<Movie, EmptyContext>>>(new[] { success.Object, failure.Object });

                // Act
                EntitiesBuilder = Mock.Create<EntitiesBuilder<Movie, EmptyContext>>();
                var entities = await EntitiesBuilder.BuildMany();

                // Assert
                EntitiesBuilder.Successful
                    .Should()
                    .BeFalse();
                entities
                    .Should()
                    .HaveSameCount(builtMovies)
                    .And
                    .Contain(builtMovies);
            }

            private static Mock<IBuildStep<Movie, EmptyContext>> CreateBuildStepMock(Result<Movie[]> result, int position)
            {
                var stepMock = new Mock<IBuildStep<Movie, EmptyContext>>();

                stepMock
                    .Setup(step => step.BuildManyAsync(It.IsAny<Movie[]>(), It.IsAny<EmptyContext>()))
                    .ReturnsAsync(result);
                stepMock
                    .SetupGet(step => step.Enabled)
                    .Returns(true);
                stepMock
                    .SetupGet(step => step.Position)
                    .Returns(position);
                return stepMock;
            }
        }
    }
}