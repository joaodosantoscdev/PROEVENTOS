using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using ProEventos.Persistence.Repository.Interfaces;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Services
{
    public class EventSevice : IEventService
    {
        // Constructor &  Dependencies
        #region DI Injected 
        private readonly IGeneralRepository _generalRepository;
        private readonly IEventRepository _eventRepository;

        public EventSevice(IGeneralRepository generalRepository, IEventRepository eventRepository)
        {
            _generalRepository = generalRepository;
            _eventRepository = eventRepository;
        }
        #endregion

        // ADD - Events
        #region POST Methods - Services
        public async Task<Event> AddEvents(Event model)
        {
            try 
            {
                _generalRepository.Add<Event>(model);
                if (await _generalRepository.SaveChangesAsync())
                {
                    return await _eventRepository.GetEventByIdAsync(model.Id, false);
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
        public async Task<Event> UpdateEvent(int eventId, Event model)
        {
            try
            {
                var _event = await _eventRepository.GetEventByIdAsync(eventId, false);
                if (_event == null) return null;

                model.Id = _event.Id;

                _generalRepository.Update(model);

                if (await _generalRepository.SaveChangesAsync())
                {
                    return await _eventRepository.GetEventByIdAsync(model.Id, false);
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
        public async Task<bool> DeleteEvent(int eventId)
        {
            try
            {
                var _event = await _eventRepository.GetEventByIdAsync(eventId);
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
        public async Task<Event[]> GetAllEventsAsync(bool includeSpeaker = false)
        {
            try
            {
                var _events = await _eventRepository.GetAllEventsAsync(includeSpeaker);
                if (_events == null) return null;

                return _events;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<Event> GetEventByIdAsync(int eventId, bool includeSpeaker = false)
        {
            try
            {
                var _events = await _eventRepository.GetEventByIdAsync(eventId, includeSpeaker);
                if (_events == null) return null;

                return _events;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<Event[]> GetAllEventsByThemeAsync(string theme, bool includeSpeaker = false)
        {
            try
            {
                var _events = await _eventRepository.GetAllEventsByThemeAsync(theme, includeSpeaker);
                if (_events == null) return null;

                return _events;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
