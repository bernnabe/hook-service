using System;
using System.Collections.Generic;

namespace ServiceHooks.Domain
{
    public partial class Event
    {
        public Event()
        {
            EventDetail = new HashSet<EventDetail>();
        }

        public int Id { get; set; }
        public int EventStatusId { get; set; }
        public long EntityId { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DueDate { get; set; }

        public virtual EventStatus EventStatus { get; set; }
        public virtual ICollection<EventDetail> EventDetail { get; set; }
    }
}
