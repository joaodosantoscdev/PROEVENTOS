using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Repository.Interfaces
{
    public interface ISpeakerRepository
    {
        // Speakers
        Task<Speaker[]> GetAllSpeakersByNameAsync(string name, bool includeEvents);
        Task<Speaker[]> GetAllSpeakersAsync(bool includeEvents);
        Task<Speaker> GetAllSpeakerByIdAsync(int speakerId, bool includeEvents);
    }
}
