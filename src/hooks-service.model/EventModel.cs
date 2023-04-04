using System;
using System.Collections.Generic;

namespace ServiceHooks.Model
{
    public class EventModel
    {
        public EventModel()
        {
            EventDetail = new HashSet<EventDetailModel>();
        }

        public string SuscriptionCode { get; set; }

        public long EntityId { get; set; }
        public string Username { get; set; }

        public double ValidDays { get; set; }

        public virtual ICollection<EventDetailModel> EventDetail { get; set; }
    }
}
