using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repository
{
    public class EventRepository : IEventRepository
    {
        // Constructor &  Dependencies
        #region DI Injected

        private readonly ProEventosContext _context;
        public EventRepository(ProEventosContext context)
        {
            _context = context;
        }
        #endregion

        // Events
        #region Events Methods - Persistence
        public async Task<Event[]> GetAllEventsByThemeAsync(string theme, bool includeSpeaker = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Parts)
                .Include(e => e.SocialMedias);

            if (includeSpeaker)
            {
                query = query
                    .Include(e => e.SpeakerEvents)
                    .ThenInclude(e => e.Speaker);
            }

            query = query.OrderBy(e => e.Id).Where(e => e.Theme.ToLower().Contains(theme.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Event[]> GetAllEventsAsync(bool includeSpeaker = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Parts)
                .Include(e => e.SocialMedias);

            if (includeSpeaker)
            {
                query = query
                    .Include(e => e.SpeakerEvents)
                    .ThenInclude(e => e.Speaker);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Event> GetAllEventByIdAsync(int eventId, bool includeSpeaker = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Parts)
                .Include(e => e.SocialMedias);

            if (includeSpeaker)
            {
                query = query
                    .Include(e => e.SpeakerEvents)
                    .ThenInclude(e => e.Speaker);
            }

            query = query.OrderBy(e => e.Id == eventId);

            return await query.FirstOrDefaultAsync();
        }
        #endregion

    }
}
