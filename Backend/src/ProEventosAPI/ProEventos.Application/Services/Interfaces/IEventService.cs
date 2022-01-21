using System.Threading.Tasks;
using ProEventos.Application.DTO;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventDTO> AddEvents(int userId, EventDTO model);
        Task<EventDTO> UpdateEvent(int userId, int eventId, EventDTO model);
        Task<bool> DeleteEvent(int userId, int eventId);
        Task<PageList<EventDTO>> GetAllEventsAsync(PageParams pgParams, int userId, bool includeSpeaker = false);
        Task<EventDTO> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false);
    }   
}