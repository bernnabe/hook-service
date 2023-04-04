using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHooks.Model
{
    public class EventCreateResponseModel
    {
        public int EventId { get; set; }
        public string SuscriptionCode { get; set; }
        public IList<EventCreateResponseDetailModel> Events { get; set; }

        public EventCreateResponseModel()
        {
            Events = new List<EventCreateResponseDetailModel>();
        }
    }

    public class EventCreateResponseDetailModel
    {
        public string ReferenceCode { get; set; }
        public string UserName { get; set; }
        public string WebHookUrl { get; set; }
        public string SuscriptionDetailCode { get; set; }
    }
}
