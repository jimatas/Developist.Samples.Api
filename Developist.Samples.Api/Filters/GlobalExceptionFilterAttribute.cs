using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.MvcFilters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

using System.ComponentModel.DataAnnotations;

namespace Developist.Samples.Api.Filters
{
    public class GlobalExceptionFilterAttribute : ApiExceptionFilterAttribute
    {
        private ProblemDetailsFactory? problemDetailsFactory;

        public GlobalExceptionFilterAttribute() : base() { }

        [ActivatorUtilitiesConstructor]
        public GlobalExceptionFilterAttribute(
            IOptions<ApiExceptionFilterOptions> options,
            ProblemDetailsFactory problemDetailsFactory) : base(options)
        {
            this.problemDetailsFactory = problemDetailsFactory;
        }

        public override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception is not ApiException && !exceptionContext.ExceptionHandled)
            // Only handling the non-ApiException case here, as that one will be handled by the base class method.
            {
                var httpContext = exceptionContext.HttpContext;
                var getProblemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>;

                problemDetailsFactory ??= getProblemDetailsFactory();

                ProblemDetails problemDetails;
                if (exceptionContext.Exception is ValidationException validationException)
                {
                    problemDetails = problemDetailsFactory.CreateValidationProblemDetails(httpContext,
                        modelStateDictionary: exceptionContext.ModelState,
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: validationException.ValidationResult.ErrorMessage ?? validationException.Message);
                }
                else
                {
                    var exception = exceptionContext.Exception;
                    problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext,
                        statusCode: StatusCodes.Status500InternalServerError,
                        detail: exception.Message);
                }

                exceptionContext.Result = new ObjectResult(problemDetails);
                exceptionContext.ExceptionHandled = true;
            }
            base.OnException(exceptionContext);
        }
    }
}
