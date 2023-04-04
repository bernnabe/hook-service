using System;
using System.Collections.Generic;

namespace ServiceHooks.Domain
{
    public partial class EventStatus
    {
        public EventStatus()
        {
            Event = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Event> Event { get; set; }
    }
}
