using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using ServiceHooks.Common;
using ServiceHooks.Domain;
using ServiceHooks.Domain.Enums;
using ServiceHooks.Repository;

namespace ServiceHooks.Service
{
    public class EventResponseService : IEventResponseService
    {
        private readonly ILogger<EventService> _logger;
        private readonly ServiceHooksContext _context;

        public EventResponseService(ServiceHooksContext context, ILogger<EventService> logger)
        {
            _logger = logger;
            _context = context;
        }

        public OperationResult Create(int eventId, int eventDetailId, string userName, bool success)
        {
            _logger.LogInformation("START Event Response Create");

            using (var tx = _context.Database.BeginTransaction())
            {
                try
                {
                    string resultTypeCode = success ? Enums.EventResultTypes.OK : Enums.EventResultTypes.ERROR;

                    EventResultType eventResultType = (from er in _context.EventResultType
                                                       where er.Code.Equals(resultTypeCode)
                                                       select er).FirstOrDefault();

                    _context.EventResult.Add(new EventResult
                    {
                        EventDetailId = eventDetailId,
                        CreatedDate = DateTime.Now,
                        CreatedUser = userName,
                        EventResultTypeId = eventResultType.Id,
                    });

                    Event @event = _context.Event.Find(eventId);

                    @event.EventStatus = (from es in _context.EventStatus
                                          where es.Code.Equals(Enums.EventStatus.DONE)
                                          select es).First();

                    _context.Update(@event);
                    _context.SaveChanges();

                    tx.Commit();

                    return OperationResult.Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    tx.Rollback();
                    return OperationResult.Fail(ex.Message);
                }
                finally
                {
                    _logger.LogInformation("END Event Response Create");
                }
            }
        }
    }
}
