using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence;
using ProEventos.Domain.Models;
using ProEventos.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProEventosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        #endregion

        // GET - Events
        #region GET Methods - Controller
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var _events = await _eventService.GetAllEventsAsync(true);
                if (_events == null) return NotFound("Nenhum evento encontrado!");

                return Ok(_events);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar eventos. Erro: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var _event = await _eventService.GetEventByIdAsync(id);
                if (_event == null) return NotFound("Evento não encontrado.");

                return Ok(_event);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }

        [HttpGet("theme/{theme}")]
        public async Task<IActionResult> GetByTheme(string theme)
        {
            try
            {
                var _event = await _eventService.GetAllEventsByThemeAsync(theme);
                if (_event == null) return NotFound("Pesquisa por tema não encontrou nenhum evento(s).");

                return Ok(_event);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar eventos(s). Erro {e.Message}");
            }
        }
        #endregion

        // ADD - Events
        #region POST Methods - Controller
        [HttpPost("")]
        public async Task<IActionResult> Add(Event model)
        {
            try
            {
                var _event = await _eventService.AddEvents(model);
                if (_event == null) return BadRequest("Erro ao tentar adicionar evento(s).");

                return Ok(_event);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }
        #endregion

        //UPDATE - Events
        #region UPDATE Methods - Controller
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Event model)
        {
            try
            {
                var _event = await _eventService.UpdateEvent(id, model);
                if (_event == null) return BadRequest("Erro ao tentar atualizar evento.");

                return Ok(_event);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }
        #endregion

        //DELETE - Events
        #region DELETE Methods - Controller
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return await _eventService.DeleteEvent(id) ?
                    Ok("Deletado") :
                    BadRequest("Evento não deletado");
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }
        #endregion
    }
}
