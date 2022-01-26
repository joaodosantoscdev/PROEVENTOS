using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using ProEventos.Persistence.Repository.Interfaces;
using ProEventos.Application.DTO;
using ProEventos.Domain.Models;
using AutoMapper;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Services
{
    public class SpeakerService : ISpeakerService
    {
        // Constructor &  Dependencies
        #region DI Injected 
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMapper _mapper;

        public SpeakerService(ISpeakerRepository speakerRepository, 
                             IMapper mapper)
        {
            _mapper = mapper;
            _speakerRepository = speakerRepository;
        }
        #endregion

        // ADD - Speakers
        #region POST Methods - Services
        public async Task<SpeakerDTO> AddSpeakers(int userId, SpeakerAddDTO model)
        {
            try 
            {
                var speaker = _mapper.Map<Event>(model);
                speaker.UserId = userId;

                _speakerRepository.Add(speaker);

                if (await _speakerRepository.SaveChangesAsync())
                {
                    var speakerReturn = await _speakerRepository.GetSpeakerByUserIdAsync(userId, false);

                    return _mapper.Map<SpeakerDTO>(speakerReturn);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // UPDATE - Speakers
        #region UPDATE Methods - Services
        public async Task<SpeakerDTO> UpdateSpeaker(int userId, SpeakerUpdateDTO model)
        {
            try
            {
                var speaker = await _speakerRepository.GetSpeakerByUserIdAsync(userId, false);
                if (speaker == null) return null;

                model.Id = speaker.Id;
                model.UserId = userId;

                _mapper.Map(model, speaker);

                _speakerRepository.Update(speaker);

                if (await _speakerRepository.SaveChangesAsync())
                {
                    var speakerResult = await _speakerRepository.GetSpeakerByUserIdAsync(userId, false);

                    return _mapper.Map<SpeakerDTO>(speakerResult);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception (e.Message);
            }
        }
        #endregion

        // GET - Speakers
        #region GET Methods - Services
        public async Task<PageList<SpeakerDTO>> GetAllSpeakersAsync(PageParams pgParams, bool includeEvents = false)
        {
            try
            {
                var speakers = await _speakerRepository.GetAllSpeakersAsync(pgParams, includeEvents);
                if (speakers == null) return null;

                var result = _mapper.Map<PageList<SpeakerDTO>>(speakers);

                result.CurrentPage = speakers.CurrentPage;
                result.TotalPages = speakers.TotalPages;
                result.PageSize = speakers.PageSize;
                result.TotalCount = speakers.TotalCount;

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<SpeakerDTO> GetSpeakerByUserIdAsync(int userId, bool includeEvents = false)
        {
            try
            {
                var speaker = await _speakerRepository.GetSpeakerByUserIdAsync(userId, includeEvents);
                if (speaker == null) return null;

                var result = _mapper.Map<SpeakerDTO>(speaker);

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
