using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence
{
    public interface IGeneralRepository
    {
        // Geral
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Events
        Task<Event[]> GetAllEventsByThemeAsync(string theme, bool includeSpeaker);
        Task<Event[]> GetAllEventsAsync(bool includeSpeaker);
        Task<Event> GetAllEventByIdAsync(int eventId, bool includeSpeaker);

        // Speakers
        Task<Event[]> GetAllSpeakersByNameAsync(string name, bool includeEvents);
        Task<Event[]> GetAllSpeakersAsync(bool includeEvents);
        Task<Event> GetAllSpeakerByIdAsync(int speakerId, bool includeEvents);
    }
}
