using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Repository.Interfaces
{
    public interface ISpeakerRepository : IGeneralRepository
    {
        // Speakers
        Task<PageList<Speaker>> GetAllSpeakersAsync(PageParams pgParams, bool includeEvents = false);
        Task<Speaker> GetSpeakerByUserIdAsync(int userId, bool includeEvents = false);
    }
}
