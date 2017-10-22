using FluentAssertions;
using Infrastructure.DataProviders.Timeline;
using System;
using TestsCore;
using Xunit;

namespace NaCoDoKina.Api.DataProviders.Timeline
{
    public class LimitedTimelineTest : UnitTestBase
    {
        public class Move : LimitedTimelineTest
        {
            [Fact]
            public void Should_change_position_by_default_step()
            {
                // Arrange
                var defaultStep = TimeSpan.FromHours(1);
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var timeline = new LimitedTimeline(start, end, defaultStep);

                // Act
                timeline.Move();

                // Assert
                timeline.Position
                    .Should()
                    .BeExactly(defaultStep);
            }

            [Fact]
            public void Should_change_position_by_given_step()
            {
                // Arrange
                var defaultStep = TimeSpan.FromHours(1);
                var givenStep = TimeSpan.FromHours(2);
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var timeline = new LimitedTimeline(start, end, defaultStep);

                // Act
                timeline.Move(givenStep);

                // Assert
                timeline.Position
                    .Should()
                    .BeExactly(givenStep);
            }

            [Fact]
            public void Should_change_position_to_given_position()
            {
                // Arrange
                var defaultStep = TimeSpan.FromHours(1);
                var givenPosition = DateTime.Today.AddHours(5);
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var timeline = new LimitedTimeline(start, end, defaultStep);

                // Act
                timeline.Move(givenPosition);

                // Assert
                timeline.Position
                    .Should()
                    .Be(givenPosition);
            }
        }

        public class IsInRange : LimitedTimelineTest
        {
            [Fact]
            public void Should_be_in_range_when_position_within_range()
            {
                // Arrange
                var defaultStep = TimeSpan.FromHours(1);
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var timeline = new LimitedTimeline(start, end, defaultStep);

                // Act
                timeline.Move();

                // Assert
                timeline.IsInRange()
                    .Should()
                    .BeTrue();
            }

            [Fact]
            public void Should_not_be_in_range_when_position_outside_range()
            {
                // Arrange
                var defaultStep = TimeSpan.FromHours(1);
                var givenStep = TimeSpan.FromDays(10);
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var timeline = new LimitedTimeline(start, end, defaultStep);

                // Act
                timeline.Move(givenStep);

                // Assert
                timeline.IsInRange()
                    .Should()
                    .BeFalse();
            }
        }
    }
}