using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
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
        public async Task<Event[]> GetAllEventsByThemeAsync(int userId, string theme, bool includeSpeaker = false)
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

            query = query.AsNoTracking().OrderBy(e => e.Id)
                         .Where(e => e.Theme.ToLower().Contains(theme.ToLower()) && 
                                     e.UserId == userId);

            return await query.ToArrayAsync();
        }

        public async Task<Event[]> GetAllEventsAsync(int userId, bool includeSpeaker = false)
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

            query = query.AsNoTracking().Where(e => e.UserId == userId).OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Event> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false)
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

            query = query.AsNoTracking().OrderBy(e => e.Id)
                .Where(e => e.Id == eventId 
                         && e.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
        #endregion

    }
}
