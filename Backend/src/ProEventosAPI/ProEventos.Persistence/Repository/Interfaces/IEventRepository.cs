using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Repository.Interfaces
{
    public interface IEventRepository
    {
        // Events
        Task<Event[]> GetAllEventsByThemeAsync(string theme, bool includeSpeaker);
        Task<Event[]> GetAllEventsAsync(bool includeSpeaker);
        Task<Event> GetAllEventByIdAsync(int eventId, bool includeSpeaker);
    }
}
