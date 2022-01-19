using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventDTO> AddEvents(int userId, EventDTO model);
        Task<EventDTO> UpdateEvent(int userId, int eventId, EventDTO model);
        Task<bool> DeleteEvent(int userId, int eventId);
        Task<EventDTO[]> GetAllEventsByThemeAsync(int userId, string theme, bool includeSpeaker = false);
        Task<EventDTO[]> GetAllEventsAsync(int userId, bool includeSpeaker = false);
        Task<EventDTO> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false);
    }   
}