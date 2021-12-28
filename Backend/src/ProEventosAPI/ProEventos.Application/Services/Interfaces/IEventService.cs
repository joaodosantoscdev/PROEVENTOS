using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Services.Interfaces
{
    public interface IEventService
    {
        Task<Event> AddEvents(Event model);
        Task<Event> UpdateEvent(int eventId, Event model);
        Task<bool> DeleteEvent(int eventId);
        Task<Event[]> GetAllEventsByThemeAsync(string theme, bool includeSpeaker = false);
        Task<Event[]> GetAllEventsAsync(bool includeSpeaker = false);
        Task<Event> GetEventByIdAsync(int eventId, bool includeSpeaker = false);
    }   
}