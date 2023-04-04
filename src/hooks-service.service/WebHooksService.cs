using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ServiceHooks.Common;
using ServiceHooks.Domain;
using ServiceHooks.Domain.Enums;
using ServiceHooks.Repository;

namespace ServiceHooks.Service
{
    public class WebHooksService : IWebHooksService
    {
        private readonly ILogger<WebHooksService> _logger;
        private readonly ServiceHooksContext _context;
        private readonly HttpClient _httpClientFactory;
        private readonly IEventService _eventService;
        private readonly IEventResponseService _eventResponseService;

        public WebHooksService(ServiceHooksContext context,
            HttpClient httpClientFactory,
            ILogger<WebHooksService> logger,
            IEventService eventService,
            IEventResponseService eventResponseService)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _eventService = eventService;
            _eventResponseService = eventResponseService;
        }

        public OperationResult Do(string code)
        {
            _logger.LogInformation("START WebHooks Do");

            try
            {
                var eventDetail = (from e in _context.Event
                                   join ed in _context.EventDetail on e.Id equals ed.EventId
                                   where ed.Code.Equals(code)
                                   select ed)
                  .Include(ed => ed.SuscriptionDetail).ThenInclude(c => c.Suscription)
                  .Include(e => e.Event)
                  .FirstOrDefault();

                if (eventDetail == null)
                {
                    return OperationResult.Fail($"Code: '{code}' doesn't exists");
                }

                var validationResult = Validate(eventDetail);

                if (!validationResult.Succeded)
                {
                    return validationResult;
                }

                var hookupResult = HookUp(eventDetail);

                _eventResponseService.Create(eventDetail.EventId, eventDetail.Id, eventDetail.Username, hookupResult.Succeded);

                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                //_eventService.AddResponse(eventDetail.Id, eventDetail.Username, false);
                _logger.LogError(ex, ex.Message);

                return OperationResult.Fail("Fatal Error");
            }
            finally
            {
                _logger.LogInformation("END WebHooks Do");
            }
        }

        private OperationResult Validate(EventDetail eventDetail)
        {
            bool eventExists = (from r in _context.Event
                                join s in _context.EventStatus on r.EventStatusId equals s.Id
                                join rd in _context.EventDetail on r.Id equals rd.EventId
                                join aa in _context.SuscriptionDetail on rd.SuscriptionDetailId equals aa.Id
                                where s.Code.Equals(Enums.EventStatus.PENDING)
                                where aa.Suscription.Code.Equals(eventDetail.SuscriptionDetail.Suscription.Code)
                                where r.EntityId.Equals(eventDetail.Event.EntityId)
                                select r).Any();

            if (!eventExists)
            {
                return OperationResult.Fail($"The event doesn't exists or it is invalid");
            }

            return OperationResult.Ok();
        }

        private OperationResult HookUp(EventDetail eventDetail)
        {
            _logger.LogInformation($"START Hookup");

            try
            {
                _logger.LogInformation($"Request POST URL: {eventDetail.SuscriptionDetail.WebHookUrl}");

                var values = new
                {
                    ReferenceCode = eventDetail.Code,
                    SuscriptionDetailCode = eventDetail.SuscriptionDetail.Code,
                    EntityId = eventDetail.Event.EntityId.ToString(),
                    Username = eventDetail.Username,
                    Datetime = DateTime.Now.ToString()
                };

                var webhookUri = new Uri(eventDetail.SuscriptionDetail.WebHookUrl);
                var response = _httpClientFactory.PostAsJsonAsync(webhookUri, values).Result;

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(response.ReasonPhrase);

                    return OperationResult.Fail("Webhooks has been rejected");
                }

                _logger.LogInformation($"Response Ok");
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return OperationResult.Fail("Fatal Error");
            }
            finally
            {
                _logger.LogInformation($"END Hookup");
            }
        }
    }
}
