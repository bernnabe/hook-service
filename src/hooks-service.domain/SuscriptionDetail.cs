using System;
using System.Collections.Generic;

namespace ServiceHooks.Domain
{
    public partial class SuscriptionDetail
    {
        public SuscriptionDetail()
        {
            EventDetail = new HashSet<EventDetail>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int SuscriptionId { get; set; }
        public int SuscriptionTypeId { get; set; }
        public string WebHookUrl { get; set; }

        public virtual Suscription Suscription { get; set; }
        public virtual SuscriptionType SuscriptionType { get; set; }
        public virtual ICollection<EventDetail> EventDetail { get; set; }
    }
}
