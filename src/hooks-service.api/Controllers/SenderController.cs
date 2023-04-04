using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceHooks.Common;
using ServiceHooks.Domain;
using ServiceHooks.Model;
using ServiceHooks.Repository;
using ServiceHooks.Service;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.Extensions.Logging;

namespace ServiceHooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SenderController : ServiceHooksApiController
    {
        private readonly IEventService _eventService;
        private readonly ILogger<SenderController> _logger;


        public SenderController(IMapper mapper, IEventService eventService, ILogger<SenderController> logger)
            : base(mapper)
        {
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
        {
            return await _eventService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            Event @event = await _eventService.Find(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        [HttpPost]
        public async Task<ActionResult<EventCreateResponseModel>> Post(EventModel requestModel)
        {
            _logger.LogInformation($"START SenderController POST - SuscriptionCode: {requestModel.SuscriptionCode}");

            if (requestModel.EntityId == 0) return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status428PreconditionRequired, "EntityId must be greater than zero");
            if (string.IsNullOrWhiteSpace(requestModel.SuscriptionCode)) return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status428PreconditionRequired, "ApplicationActionCode must contains a value");
            if (string.IsNullOrWhiteSpace(requestModel.Username)) return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status428PreconditionRequired, "Username must contains a value");
            if (requestModel.EventDetail.Count == 0) return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status428PreconditionRequired, "Detail must contains a value");
            if (requestModel.EventDetail.ToList().Exists(c => string.IsNullOrEmpty(c.CreatedUser))) return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status428PreconditionRequired, "Invalid username in detail");

            try
            {
                var result = _eventService.Create(requestModel);

                if (!result.Succeded)
                {
                    return Conflict(result.Errors);
                }

                return CreatedAtAction("GetEvent", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Fatal Error");
            }
            finally
            {
                _logger.LogInformation($"END SenderController POST - SuscriptionCode: {requestModel.SuscriptionCode}");
            }
        }
    }
}
