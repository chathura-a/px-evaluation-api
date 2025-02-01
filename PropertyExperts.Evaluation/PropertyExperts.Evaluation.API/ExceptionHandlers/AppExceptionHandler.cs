using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PropertyExperts.Evaluation.API.ExceptionHandlers
{
    public class AppExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> logger;

        public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problemDetails = new()
            {
                Type = exception.GetType().Name,
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
            };

            problemDetails.Status = exception switch
            {
                HttpRequestException => StatusCodes.Status503ServiceUnavailable,
                BadHttpRequestException => StatusCodes.Status400BadRequest,
                _ => (int?)StatusCodes.Status500InternalServerError,
            };

            if(problemDetails.Status == StatusCodes.Status500InternalServerError)
            {
                problemDetails.Detail = "An unexpected error occured.";
            }

            httpContext.Response.StatusCode = (int)problemDetails.Status;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            this.logger.LogError("{exception}", JsonConvert.SerializeObject(problemDetails));
            return true;
        }
    }
}
