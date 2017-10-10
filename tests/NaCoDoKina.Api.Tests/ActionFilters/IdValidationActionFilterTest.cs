using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using NaCoDoKina.Api.Controllers;
using System.Collections.Generic;
using TestsCore;
using Xunit;

namespace NaCoDoKina.Api.ActionFilters
{
    public class IdValidationActionFilterTest : UnitTestBase
    {
        private IActionFilter ActionFilterUnderTest { get; }

        public IdValidationActionFilterTest()
        {
            ActionFilterUnderTest = new IdValidationActionFilter();
        }

        [Fact]
        public void Should_return_error_for_each_action_argument_with_le_0_id()
        {
            // Arrange
            var actionArguments = new Dictionary<string, object>
            {
                ["id"] = 0L,
                ["movieId"] = 2L,
                ["cinemaId"] = -3L,
                ["wordWithIdInTheMiddle"] = -1L,
            };

            var executingContext = new ActionExecutingContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor(),
                    new ModelStateDictionary()),
                new List<IFilterMetadata>(),
                actionArguments,
                Mock.Create<MoviesController>());

            // Act
            ActionFilterUnderTest.OnActionExecuting(executingContext);

            //Assert
            executingContext.ModelState.ErrorCount
                .Should().Be(2);

            executingContext.ModelState.ValidationState
                .Should().HaveFlag(ModelValidationState.Invalid);
        }
    }
}