using ProEventos.Application.DTO;
using ProEventos.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Services.Interfaces
{
    public interface ISpeakerService
    {
        Task<SpeakerDTO> AddSpeakers(int userId, SpeakerAddDTO model);
        Task<SpeakerDTO> UpdateSpeaker(int userId, SpeakerUpdateDTO model);
        Task<PageList<SpeakerDTO>> GetAllSpeakersAsync(PageParams pgParams, bool includeEvents = false);
        Task<SpeakerDTO> GetSpeakerByUserIdAsync(int userId, bool includeEvents = false);
    }
}
