using AutoMapper;
using ServiceHooks.Domain;
using ServiceHooks.Model;

namespace ServiceHooks.Api.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventModel, Event>();
            CreateMap<Event, EventModel>();

            CreateMap<EventDetailModel, EventDetail>();
            CreateMap<EventDetail, EventDetailModel>();
        }
    }
}
