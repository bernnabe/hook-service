using System;
using System.Collections.Generic;

namespace ServiceHooks.Domain
{
    public partial class EventResult
    {
        public int Id { get; set; }
        public int EventDetailId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public int EventResultTypeId { get; set; }

        public virtual EventDetail EventDetail { get; set; }
        public virtual EventResultType EventResultType { get; set; }
    }
}
