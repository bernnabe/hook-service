using System;
using System.Collections.Generic;

namespace ServiceHooks.Model
{
    public partial class EventDetailModel
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
    }
}
