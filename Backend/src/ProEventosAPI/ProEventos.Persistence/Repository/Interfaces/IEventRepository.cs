using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Repository.Interfaces
{
    public interface IEventRepository
    {
        // Events
        Task<PageList<Event>> GetAllEventsAsync(PageParams pgParams, int userId, bool includeSpeaker = false);
        Task<Event> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false);
    }
}
