using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class GeneralRepository : IGeneralRepository
    {
        // Constructor &  Dependencies
        #region DI Injected

        private readonly ProEventosContext _context;
        public GeneralRepository(ProEventosContext context)
        {
            _context = context;
        }
        #endregion

        //Geral 
        #region Geral Methods - Persistence
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
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

        // Speakers 
        public async Task<Speaker[]> GetAllSpeakersByNameAsync(string name, bool includeEvents)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(p => p.SocialMedias);

            if (includeEvents)
            {
                query = query
                    .Include(p => p.SpeakerEvents)
                    .ThenInclude(pe => pe.Event);
            }

            query = query.OrderBy(p => p.Id).Where(p => p.Name.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Speaker[]> GetAllSpeakersAsync(bool includeEvents)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(p => p.SocialMedias);

            if (includeEvents)
            {
                query = query
                    .Include(p => p.SpeakerEvents)
                    .ThenInclude(pe => pe.Event);
            }

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Speaker> GetAllSpeakerByIdAsync(int speakerId, bool includeEvents)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(e => e.SocialMedias);

            if (includeEvents)
            {
                query = query
                    .Include(e => e.SpeakerEvents)
                    .ThenInclude(e => e.Speaker);
            }

            query = query.OrderBy(e => e.Id == speakerId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
