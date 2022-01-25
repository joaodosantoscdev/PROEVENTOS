using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Models;
using ProEventos.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repository
{
    public class SpeakerRepository : GeneralRepository, ISpeakerRepository
    {
        // Constructor &  Dependencies
        #region DI Injected

        private readonly ProEventosContext _context;
        public SpeakerRepository(ProEventosContext context) : base (context)
        {
            _context = context;
        }
        #endregion

        // Speakers 
        #region Speakers Methods - Persistence
        public async Task<PageList<Speaker>> GetAllSpeakersAsync(PageParams pgParams, bool includeEvents = false)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(p => p.User)
                .Include(p => p.SocialMedias);

            if (includeEvents)
            {
                query = query.AsNoTracking()
                    .Include(p => p.SpeakerEvents)
                    .ThenInclude(pe => pe.Event);
            }

            query = query.Where(p => (p.CV.ToLower().Contains(pgParams.Term.ToLower()) ||
                                      p.User.FirstName.ToLower().Contains(pgParams.Term.ToLower()) ||
                                      p.User.LastName.ToLower().Contains(pgParams.Term.ToLower())) &&
                                      p.User.Function == Domain.Enum.Function.Palestrante)
                         .OrderBy(p => p.Id);

            return await PageList<Speaker>.CreateAsync(query, pgParams.PageNumber, pgParams.pageSize);
        }

        public async Task<Speaker> GetAllSpeakerByIdAsync(int userId, bool includeEvents)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(p => p.User)
                .Include(e => e.SocialMedias);

            if (includeEvents)
            {
                query = query.AsNoTracking()
                    .Include(e => e.SpeakerEvents)
                    .ThenInclude(e => e.Speaker);
            }

            query = query.OrderBy(e => e.Id == userId);

            return await query.FirstOrDefaultAsync();
        }
        #endregion

    }
}
