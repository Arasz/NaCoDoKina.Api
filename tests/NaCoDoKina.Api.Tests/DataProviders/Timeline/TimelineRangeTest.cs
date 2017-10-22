using FluentAssertions;
using Infrastructure.DataProviders.Timeline;
using System;
using System.Collections.Generic;
using TestsCore;
using Xunit;

namespace NaCoDoKina.Api.DataProviders.Timeline
{
    public class TimelineRangeTest : UnitTestBase
    {
        public TimelineRangeTest()
        {
            Start = DateTime.Today;
            End = DateTime.Today.AddDays(5);
        }

        public DateTime End { get; set; }

        public DateTime Start { get; set; }

        public class IsInRange : TimelineRangeTest
        {
            public static IEnumerable<object[]> TimelinePositionGenerator()
            {
                var today = DateTime.Today;

                yield return new[] { (object)today };
                yield return new[] { (object)today.AddHours(2) };
                yield return new[] { (object)today.AddDays(1) };
                yield return new[] { (object)today.AddDays(2) };
                yield return new[] { (object)today.AddDays(5) };
            }

            [Theory]
            [MemberData(nameof(TimelinePositionGenerator))]
            public void When_less_or_equal_than_end_and_greater_or_equal_than_start_should_be_true(DateTime position)
            {
                // Arrange
                var timelineRange = new TimelineRange(Start, End);

                // Act
                var isInRange = timelineRange.IsInRange(position);

                // Assert

                isInRange.Should().BeTrue();
            }
        }
    }
}