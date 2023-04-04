using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ServiceHooks.Common;
using ServiceHooks.Service;

namespace ServiceHooks.Api.Controllers
{
    [Route("/{code}")]
    [ApiController]
    //https://github.com/aspnet/AspNetCore.Docs/blob/master/aspnetcore/fundamentals/http-requests/samples/3.x/HttpClientFactorySample/GitHub/GitHubService.cs
    public class ReceiverController : ServiceHooksApiController
    {
        private readonly ILogger<ReceiverController> _logger;
        private readonly IWebHooksService _webHooksService;

        public ReceiverController(IWebHooksService webHooksService, ILogger<ReceiverController> logger, IMapper mapper)
            : base(mapper)
        {
            _logger = logger;
            _webHooksService = webHooksService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Post([FromRoute]string code)
        {
            _logger.LogInformation($"START Receiver with code {code}");

            if (string.IsNullOrWhiteSpace(code))
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status428PreconditionRequired, "Code can't be empty");
            }

            try
            {
                OperationResult result = _webHooksService.Do(code);

                _logger.LogInformation($"END Receiver with code {code}");

                if (!result.Succeded)
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict, result.Errors);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Fatal Error");
            }
            finally
            {
                _logger.LogInformation($"END Receiver with code {code}");
            }
        }
    }
}