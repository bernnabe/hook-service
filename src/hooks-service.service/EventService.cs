using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceHooks.Common;
using ServiceHooks.Domain;
using ServiceHooks.Domain.Enums;
using ServiceHooks.Model;
using ServiceHooks.Repository;

namespace ServiceHooks.Service
{
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly ServiceHooksContext _context;
        private readonly IMapper _mapper;

        public EventService(ServiceHooksContext context, IMapper mapper, ILogger<EventService> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public OperationResult<EventCreateResponseModel> Create(EventModel eventModel)
        {
            _logger.LogInformation("START Event Create");

            const double DEFAULT_VALID_DAYS = 30;

            using (var tx = _context.Database.BeginTransaction())
            {
                try
                {
                    Event @event = new Event
                    {
                        EntityId = eventModel.EntityId,
                        EventStatusId = _context.EventStatus.Where(c => c.Code.Equals(Enums.EventStatus.PENDING)).First().Id,
                        CreatedUser = eventModel.Username,
                        CreatedDate = DateTime.Now,
                        DueDate = eventModel.ValidDays <= 0 ? DateTime.Now.AddDays(DEFAULT_VALID_DAYS) : DateTime.Now.AddDays(eventModel.ValidDays).Date, //TODO: Cambiar esto por 
                    };

                    OperationResult validationResult = Validate(eventModel);

                    if (!validationResult.Succeded)
                    {
                        return OperationResult<EventCreateResponseModel>.Fail(validationResult.Errors);
                    }

                    var suscription = _context.Suscription
                        .Where(c => c.Code.Equals(eventModel.SuscriptionCode))
                        .Include(c => c.SuscriptionDetail)
                        .FirstOrDefault();

                    if (suscription == null)
                    {
                        return OperationResult<EventCreateResponseModel>.Fail($"ApplicationActionCode '{eventModel.SuscriptionCode}' doesn't exist");
                    }

                    EventCreateResponseModel result = new EventCreateResponseModel
                    {
                        EventId = @event.Id,
                        SuscriptionCode = suscription.Code,
                    };

                    foreach (var modelDetail in eventModel.EventDetail)
                    {
                        foreach (var detail in suscription.SuscriptionDetail)
                        {
                            string eventDetailCode = Guid.NewGuid().ToString("N");

                            @event.EventDetail.Add(new EventDetail
                            {
                                SuscriptionDetailId = detail.Id,
                                Code = eventDetailCode,
                                Username = modelDetail.CreatedUser,
                            });

                            result.Events.Add(new EventCreateResponseDetailModel
                            {
                                ReferenceCode = eventDetailCode,
                                SuscriptionDetailCode = detail.Code,
                                UserName = modelDetail.CreatedUser,
                                WebHookUrl = detail.WebHookUrl,
                            });
                        }
                    }

                    _context.Event.Include(b => b.EventDetail);
                    _context.Event.Add(@event);
                    _context.SaveChanges();

                    tx.Commit();

                    return OperationResult<EventCreateResponseModel>.Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    tx.Rollback();

                    return OperationResult<EventCreateResponseModel>.Fail("Fatal Error");
                }
                finally
                {
                    _logger.LogInformation("END Event Create");
                }
            }
        }

        private OperationResult Validate(EventModel eventModel)
        {
            bool alreadyExistsEvent = (from r in _context.Event
                                       join s in _context.EventStatus on r.EventStatusId equals s.Id
                                       join rd in _context.EventDetail on r.Id equals rd.EventId
                                       join aa in _context.SuscriptionDetail on rd.SuscriptionDetailId equals aa.Id
                                       where s.Code.Equals(Enums.EventStatus.PENDING)
                                       where aa.Suscription.Code.Equals(eventModel.SuscriptionCode)
                                       where r.EntityId.Equals(eventModel.EntityId)
                                       select r).Any();

            if (alreadyExistsEvent)
            {
                return OperationResult.Fail($"Already exists a event in pending status for EntityId '{eventModel.EntityId}' - ApplicationCode '{eventModel.SuscriptionCode}'");
            }

            return OperationResult.Ok();
        }

        public bool Exists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }

        public async Task<Event> Delete(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return null;
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();

            return @event;
        }

        public Task<List<Event>> GetAll()
        {
            return _context.Event.ToListAsync();
        }

        public Task<Event> Find(int id)
        {
            return _context.Event.FindAsync(id).AsTask();
        }

        public async Task<int> Update(int id, EventModel eventModel)
        {
            throw new NotImplementedException();
        }
    }
}
