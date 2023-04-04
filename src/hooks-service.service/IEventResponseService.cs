using System;
using System.Collections.Generic;
using System.Text;
using ServiceHooks.Common;

namespace ServiceHooks.Service
{
    public interface IEventResponseService
    {
        OperationResult Create(int eventId, int eventDetailId, string userName, bool success);
    }
}
