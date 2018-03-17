using Homely.AspNetCore.Mvc.Helpers.ActionFilters;
using Homely.AspNetCore.Mvc.Helpers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public class ValidateModelAttributeTests
    {
        [Fact]
        public void GivenAnInvalidModelState_ValidateModelAttribute_ReturnsNoResult()
        {
            // Arrange.
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext,
                                                  new RouteData(),
                                                  new ActionDescriptor());

            var context = new ActionExecutingContext(actionContext,
                                                     new List<IFilterMetadata>(),
                                                     new Dictionary<string, object>(),
                                                     new Mock<Controller>().Object);

            var attribute = new ValidateModelAttribute();

            // Act.
            attribute.OnActionExecuting(context);

            // Assert.
            context.Result.ShouldBeNull();
        }

        [Fact]
        public void GivenAnInvalidModelState_ValidateModelAttribute_ReturnsBadRequestObjectResult()
        {
            // Arrange.
            var httpContext = new DefaultHttpContext();
            
            const string key = "name";
            const string message = "missing Name";
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(key, message);
            
            var actionContext = new ActionContext(httpContext,
                                                  new RouteData(),
                                                  new ActionDescriptor(),
                                                  modelStateDictionary);

            var context = new ActionExecutingContext(actionContext,
                                                     new List<IFilterMetadata>(),
                                                     new Dictionary<string, object>(),
                                                     new Mock<Controller>().Object);

            var attribute = new ValidateModelAttribute();

            // Act.
            attribute.OnActionExecuting(context);

            // Assert.
            context.Result.ShouldNotBeNull();
            context.Result.ShouldBeOfType<BadRequestObjectResult>();

            var result = context.Result as BadRequestObjectResult;
            result.Value.ShouldBeOfType<ErrorViewModel>();

            var errorViewModel = result.Value as ErrorViewModel;
            errorViewModel.Errors.Count().ShouldBe(1);

            var apiError = errorViewModel.Errors.First();
            apiError.Key.ShouldBe(key);
            apiError.Message.ShouldBe(message);
        }
    }
}
