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
    public class PartRepository : IPartRepository
    {
        // Constructor &  Dependencies
        #region DI Injected

        private readonly ProEventosContext _context;
        public PartRepository(ProEventosContext context)
        {
            _context = context;
        }
        #endregion

        // Parts
        #region Parts Methods - Persistence
        public async Task<Part[]> GetAllPartsByEventIdAsync(int eventId)
        {
            IQueryable<Part> query = _context.Parts;

            query = query.AsNoTracking()
                .Where(p => p.EventId == eventId);

            return await query.ToArrayAsync();
        }

        public async Task<Part> GetPartByIdsAsync(int eventId, int id)
        {
            IQueryable<Part> query = _context.Parts;

            query = query.AsNoTracking()
                .Where(p => p.EventId == eventId 
                         && p.Id == id);

             return await query.FirstOrDefaultAsync();
        }
        #endregion

    }
}
