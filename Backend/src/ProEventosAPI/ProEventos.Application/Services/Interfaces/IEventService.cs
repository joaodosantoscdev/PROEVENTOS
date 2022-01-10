using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventDTO> AddEvents(EventDTO model);
        Task<EventDTO> UpdateEvent(int eventId, EventDTO model);
        Task<bool> DeleteEvent(int eventId);
        Task<EventDTO[]> GetAllEventsByThemeAsync(string theme, bool includeSpeaker = false);
        Task<EventDTO[]> GetAllEventsAsync(bool includeSpeaker = false);
        Task<EventDTO> GetEventByIdAsync(int eventId, bool includeSpeaker = false);
    }   
}