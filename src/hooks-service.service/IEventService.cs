using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServiceHooks.Common;
using ServiceHooks.Domain;
using ServiceHooks.Model;

namespace ServiceHooks.Service
{
    public interface IEventService
    {
        OperationResult<EventCreateResponseModel> Create(EventModel requestModel);
        bool Exists(int id);
        Task<List<Event>> GetAll();
        Task<Event> Find(int id);
        Task<Event> Delete(int id);
        Task<int> Update(int id, EventModel requestModel);
    }
}
