using InCorpApp.Application.Utilities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InCorpApp.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("/error")]
        [HttpPost]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            logger.LogError($"\n[Exception] - {JsonConvert.SerializeObject(exception?.Message + "\n" + exception?.StackTrace)}\n");
            var response = ResponseBuilder.Build<object>(statusCode: System.Net.HttpStatusCode.InternalServerError, hasError: true, actionMessage: "Unexpected Error Occured. Please try again");
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [Route("/error")]
        [HttpGet]
        public IActionResult ErrorGet()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            logger.LogError($"\n[Exception] - {JsonConvert.SerializeObject(exception?.Message + "\n" + exception?.StackTrace)}\n");
            var response = ResponseBuilder.Build<object>(statusCode: System.Net.HttpStatusCode.InternalServerError, hasError: true, actionMessage: "Unexpected Error Occured. Please try again");
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
