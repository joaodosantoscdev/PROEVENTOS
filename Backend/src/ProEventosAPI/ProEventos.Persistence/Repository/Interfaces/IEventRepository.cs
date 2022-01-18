using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Repository.Interfaces
{
    public interface IEventRepository
    {
        // Events
        Task<Event[]> GetAllEventsByThemeAsync(string theme, bool includeSpeaker = false);
        Task<Event[]> GetAllEventsAsync(bool includeSpeaker = false);
        Task<Event> GetEventByIdAsync(int eventId, bool includeSpeaker = false);
    }
}
