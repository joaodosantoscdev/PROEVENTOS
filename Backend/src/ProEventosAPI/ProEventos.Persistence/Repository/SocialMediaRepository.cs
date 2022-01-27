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
    public class SocialMediaRepository : GeneralRepository, ISocialMediaRepository
    {
        private readonly ProEventosContext _context;

        public SocialMediaRepository(ProEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SocialMedia[]> GetAllByEventIdAsync(int eventId)
        {
            IQueryable<SocialMedia> query = _context.SocialMedias;

            query = query.AsNoTracking()
                         .Where(sm => sm.EventId == eventId);

            return await query.ToArrayAsync();
        }

        public async Task<SocialMedia[]> GetAllBySpeakerIdsAsync(int speakerId)
        {
            IQueryable<SocialMedia> query = _context.SocialMedias;

            query = query.AsNoTracking()
                         .Where(sm => sm.SpeakerId == speakerId);

            return await query.ToArrayAsync();
        }

        public async Task<SocialMedia> GetEventsSocialMediaByIdsAsync(int eventId, int id)
        {
            IQueryable<SocialMedia> query = _context.SocialMedias;

            query = query.AsNoTracking()
                         .Where(sm => sm.EventId == eventId &&
                                      sm.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SocialMedia> GetSpeakersSocialMediaByIdsAsync(int speakerId, int id)
        {

            IQueryable<SocialMedia> query = _context.SocialMedias;

            query = query.AsNoTracking()
                         .Where(rs => rs.SpeakerId == speakerId &&
                                          rs.Id == id);

            return await query.FirstOrDefaultAsync();

        }
    }
}
