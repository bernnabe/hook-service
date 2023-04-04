using System;
using System.Collections.Generic;

namespace ServiceHooks.Domain
{
    public partial class EventDetail
    {
        public EventDetail()
        {
            EventResult = new HashSet<EventResult>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int EventId { get; set; }
        public int SuscriptionDetailId { get; set; }
        public string Username { get; set; }

        public virtual Event Event { get; set; }
        public virtual SuscriptionDetail SuscriptionDetail { get; set; }
        public virtual ICollection<EventResult> EventResult { get; set; }
    }
}
