using ProEventos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repository.Interfaces
{
    public interface ISocialMediaRepository : IGeneralRepository
    {
        Task<SocialMedia> GetEventsSocialMediaByIdsAsync(int eventId, int id);
        Task<SocialMedia> GetSpeakersSocialMediaByIdsAsync(int speakerId, int id);
        Task<SocialMedia[]> GetAllByEventIdAsync(int eventId);
        Task<SocialMedia[]> GetAllBySpeakerIdsAsync(int speakerId);

    }
}
