using System;
using System.Collections.Generic;

namespace ServiceHooks.Domain
{
    public partial class SuscriptionType
    {
        public SuscriptionType()
        {
            SuscriptionDetail = new HashSet<SuscriptionDetail>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<SuscriptionDetail> SuscriptionDetail { get; set; }
    }
}
