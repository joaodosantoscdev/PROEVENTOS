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
    public class SpeakerRepository : ISpeakerRepository
    {
        // Constructor &  Dependencies
        #region DI Injected

        private readonly ProEventosContext _context;
        public SpeakerRepository(ProEventosContext context)
        {
            _context = context;
        }
        #endregion

        // Speakers 
        #region Speakers Methods - Persistence
        public async Task<Speaker[]> GetAllSpeakersByNameAsync(string name, bool includeEvents)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(p => p.SocialMedias);

            if (includeEvents)
            {
                query = query.AsNoTracking()
                    .Include(p => p.SpeakerEvents)
                    .ThenInclude(pe => pe.Event);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id).Where(p => p.Name.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Speaker[]> GetAllSpeakersAsync(bool includeEvents)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(p => p.SocialMedias);

            if (includeEvents)
            {
                query = query.AsNoTracking()
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
                query = query.AsNoTracking()
                    .Include(e => e.SpeakerEvents)
                    .ThenInclude(e => e.Speaker);
            }

            query = query.OrderBy(e => e.Id == speakerId);

            return await query.FirstOrDefaultAsync();
        }
        #endregion

    }
}
