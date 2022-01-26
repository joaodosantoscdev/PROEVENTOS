using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using ProEventos.Persistence.Repository.Interfaces;
using ProEventos.Application.DTO;
using ProEventos.Domain.Models;
using AutoMapper;
using System.Linq;

namespace ProEventos.Application.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        // Constructor &  Dependencies
        #region DI Injected 
        private readonly ISocialMediaRepository _socialMediaRepository;
        private readonly IMapper _mapper;

        public SocialMediaService(ISocialMediaRepository socialMediaRepository, 
                                 IMapper mapper)
        {
            _mapper = mapper;
            _socialMediaRepository = socialMediaRepository;
        }
        #endregion

        // ADD - Social Media
        #region POST Methods - Services
        public async Task AddSocialMedia(int id, SocialMediaDTO model, bool isEvent)
        {
            try 
            {
                var socialMedia = _mapper.Map<SocialMedia>(model);

                if (isEvent)
                {
                    socialMedia.EventId = id;
                    socialMedia.SpeakerId = null;
                } 
                else
                {
                    socialMedia.SpeakerId = id;
                    socialMedia.EventId = null;  
                }
                
                _socialMediaRepository.Add(socialMedia);

                await _socialMediaRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // SAVE - Social Media
        #region SAVE Methods - SocialMedia Services
        public async Task<SocialMediaDTO[]> SaveByEvent(int eventId, SocialMediaDTO[] models)
        {
            try
            {
                var _socialMedia = await _socialMediaRepository.GetAllByEventIdAsync(eventId);
                if (_socialMedia == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddSocialMedia(eventId, model, true);
                    } 
                    else
                    {
                        var socialMedia = _socialMedia.FirstOrDefault(sm => sm.Id == model.Id);
                        model.EventId = eventId;

                        _mapper.Map(model, socialMedia);

                        _socialMediaRepository.Update(socialMedia);

                        await _socialMediaRepository.SaveChangesAsync();
                    }
                }

                var socialMediaResult = await _socialMediaRepository.GetAllByEventIdAsync(eventId);

                return _mapper.Map<SocialMediaDTO[]>(socialMediaResult);
            }
            catch (Exception e)
            {
                throw new Exception (e.Message);
            }
        }

        public async Task<SocialMediaDTO[]> SaveBySpeaker(int speakerId, SocialMediaDTO[] models)
        {
            try
            {
                var _socialMedia = await _socialMediaRepository.GetAllByEventIdAsync(speakerId);
                if (_socialMedia == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddSocialMedia(speakerId, model, false);
                    }
                    else
                    {
                        var socialMedia = _socialMedia.FirstOrDefault(sm => sm.Id == model.Id);
                        model.SpeakerId = speakerId;

                        _mapper.Map(model, socialMedia);

                        _socialMediaRepository.Update(socialMedia);

                        await _socialMediaRepository.SaveChangesAsync();
                    }
                }

                var socialMediaResult = await _socialMediaRepository.GetAllByEventIdAsync(speakerId);

                return _mapper.Map<SocialMediaDTO[]>(socialMediaResult);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // DELETE - Social Media
        #region DELETE Methods - Part Services
        public async Task<bool> DeleteByEvent(int eventId, int socialMediaId)
        {
            try
            {
                var socialMedia = await _socialMediaRepository.GetEventsSocialMediaByIdsAsync(eventId, socialMediaId);
                if (socialMedia == null) throw new Exception ("Rede Social para Delete por Evento essa ação, Lote não encontrado!");

                _socialMediaRepository.Delete(socialMedia);

                return await _socialMediaRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteBySpeaker(int speakerId, int socialMediaId)
        {
            try
            {
                var socialMedia = await _socialMediaRepository.GetSpeakersSocialMediaByIdsAsync(speakerId, socialMediaId);
                if (socialMedia == null) throw new Exception("Rede Social para Delete por Palestrante essa ação, Lote não encontrado!");

                _socialMediaRepository.Delete(socialMedia);

                return await _socialMediaRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // GET - Social Media
        #region GET Methods - SocialMedia Services
        public async Task<SocialMediaDTO[]> GetAllByEventIdAsync(int eventId)
        {
            try
            {
                var socialMedias = await _socialMediaRepository.GetAllByEventIdAsync(eventId);
                if (socialMedias == null) return null;

                var result = _mapper.Map<SocialMediaDTO[]>(socialMedias);

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<SocialMediaDTO[]> GetAllBySpeakerIdAsync(int speakerId)
        {
            try
            {
                var socialMedias = await _socialMediaRepository.GetAllByEventIdAsync(speakerId);
                if (socialMedias == null) return null;

                var result = _mapper.Map<SocialMediaDTO[]>(socialMedias);

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<SocialMediaDTO> GetEventSocialMediaByIdsAsync(int eventId, int socialMediaId)
        {
            try
            {
                var socialMedias = await _socialMediaRepository.GetEventsSocialMediaByIdsAsync(eventId, socialMediaId);
                if (socialMedias == null) return null;

                var result = _mapper.Map<SocialMediaDTO>(socialMedias);

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<SocialMediaDTO> GetSpeakerSocialMediaByIdsAsync(int speakerId, int socialMediaId)
        {
            try
            {
                var socialMedias = await _socialMediaRepository.GetEventsSocialMediaByIdsAsync(speakerId, socialMediaId);
                if (socialMedias == null) return null;

                var result = _mapper.Map<SocialMediaDTO>(socialMedias);

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
