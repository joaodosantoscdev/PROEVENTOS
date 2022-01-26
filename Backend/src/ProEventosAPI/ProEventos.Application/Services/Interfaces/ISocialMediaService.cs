using ProEventos.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Services.Interfaces
{
    public interface ISocialMediaService
    {
        Task<SocialMediaDTO[]> SaveByEvent(int eventId, SocialMediaDTO[] models);

        Task<bool> DeleteByEvent(int eventId, int socialMediaId);

        Task<SocialMediaDTO[]> SaveBySpeaker(int speakerId, SocialMediaDTO[] models);

        Task<bool> DeleteBySpeaker(int speakerId, int socialMediaId);

        Task<SocialMediaDTO[]> GetAllByEventIdAsync(int eventId);

        Task<SocialMediaDTO[]> GetAllBySpeakerIdAsync(int speakerId);

        Task<SocialMediaDTO> GetEventSocialMediaByIdsAsync(int id, int eventId);

        Task<SocialMediaDTO> GetSpeakerSocialMediaByIdsAsync(int id, int speakerId);
    }
}
