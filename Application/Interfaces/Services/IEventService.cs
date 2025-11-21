using Sindika.AspNet.app015.Application.DTOs.Event;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IEventService : IBaseCrudService<EventDTO, EventPaginationDTO, EventParam>
    {

    }
}
