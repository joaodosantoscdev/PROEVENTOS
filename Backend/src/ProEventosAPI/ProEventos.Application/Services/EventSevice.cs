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
    public class EventSevice : IEventService
    {
        // Constructor &  Dependencies
        #region DI Injected 
        private readonly IGeneralRepository _generalRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventSevice(IGeneralRepository generalRepository, 
                           IEventRepository eventRepository, 
                           IMapper mapper)
        {
            _mapper = mapper;
            _generalRepository = generalRepository;
            _eventRepository = eventRepository;
        }
        #endregion

        // ADD - Events
        #region POST Methods - Services
        public async Task<EventDTO> AddEvents(int userId, EventDTO model)
        {
            try 
            {
                var _event = _mapper.Map<Event>(model);
                _event.UserId = userId;

                _generalRepository.Add(_event);

                if (await _generalRepository.SaveChangesAsync())
                {
                    var eventReturn = await _eventRepository.GetEventByIdAsync(userId, _event.Id, false);

                    return _mapper.Map<EventDTO>(eventReturn);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // UPDATE - Events
        #region UPDATE Methods - Services
        public async Task<EventDTO> UpdateEvent(int userId, int eventId, EventDTO model)
        {
            try
            {
                var _event = await _eventRepository.GetEventByIdAsync(userId, eventId, false);
                if (_event == null) return null;

                model.Id = _event.Id;
                model.UserId = userId;

                _mapper.Map(model, _event);

                _generalRepository.Update(_event);

                if (await _generalRepository.SaveChangesAsync())
                {
                    var eventResult = await _eventRepository.GetEventByIdAsync(userId, _event.Id, false);

                    return _mapper.Map<EventDTO>(eventResult);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception (e.Message);
            }
        }
        #endregion

        // DELETE - Events
        #region DELETE Methods - Services
        public async Task<bool> DeleteEvent(int userId, int eventId)
        {
            try
            {
                var _event = await _eventRepository.GetEventByIdAsync(userId, eventId, false);
                if (_event == null) throw new Exception ("Não foi possível realizar essa ação, Evento não encontrado!");

                _generalRepository.Delete<Event>(_event);

                return await _generalRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // GET - Events
        #region GET Methods - Services
        public async Task<PageList<EventDTO>> GetAllEventsAsync(PageParams pgParams, int userId, bool includeSpeaker = false)
        {
            try
            {
                var _events = await _eventRepository.GetAllEventsAsync(pgParams, userId, includeSpeaker);
                if (_events == null) return null;

                var result = _mapper.Map<PageList<EventDTO>>(_events);

                result.CurrentPage = _events.CurrentPage;
                result.TotalCount = _events.TotalCount;
                result.TotalPages = _events.TotalPages;
                result.PageSize = _events.PageSize;

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventDTO> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false)
        {
            try
            {
                var _event = await _eventRepository.GetEventByIdAsync(userId, eventId, includeSpeaker);
                if (_event == null) return null;

                var result = _mapper.Map<EventDTO>(_event);

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
