using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.DTO;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.Models;
using ProEventos.API.Helpers;

namespace ProEventosAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        private readonly IUtility _utility;
        private readonly string _destiny = "Images";
        public EventController(IEventService eventService, 
                               IUserService userService,
                               IUtility utility)
        {
            _userService = userService;
            _eventService = eventService;
            _utility = utility;
        }
        #endregion

        // GET - Events
        #region GET Methods - Controller
        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery]PageParams pgParams)
        {
            try
            {
                var _events = await _eventService.GetAllEventsAsync(pgParams, User.GetUserId(), true);
                if (_events == null) return NoContent();

                Response.AddPagination(_events.CurrentPage, _events.PageSize, _events.TotalCount, _events.TotalPages);

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
                var _event = await _eventService.GetEventByIdAsync(User.GetUserId(), id, true);
                if (_event == null) return NoContent();

                return Ok(_event);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }
        #endregion

        // ADD - Events
        #region POST Methods - Controller
        [HttpPost("")]
        public async Task<IActionResult> Add(EventDTO model)
        {
            try
            {
                var _event = await _eventService.AddEvents(User.GetUserId(), model);
                if (_event == null) return NoContent();

                return Ok(_event);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }

        [HttpPost("upload-image/{eventId}")]
        public async Task<IActionResult> UploadImg(int eventId)
        {
            try
            {
                var _event = await _eventService.GetEventByIdAsync(User.GetUserId(), eventId, true);
                if (_event == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _utility.DeleteImg(_event.ImageURL, _destiny);
                    _event.ImageURL = await _utility.SaveImg(file, _destiny);
                }
                var eventReturn = await _eventService.UpdateEvent(User.GetUserId(), eventId, _event);

                return Ok(eventReturn);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao realizar upload de foto do evento. Erro {e.Message}");
            }
        }
        #endregion

        //UPDATE - Events
        #region UPDATE Methods - Controller
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventDTO model)
        {
            try
            {
                var _event = await _eventService.UpdateEvent(User.GetUserId(), id, model);
                if (_event == null) return NoContent();

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
                var _event = await _eventService.GetEventByIdAsync(User.GetUserId(), id, true);
                if (_event == null) return NoContent();

                if (await _eventService.DeleteEvent(User.GetUserId(), id))
                {
                    _utility.DeleteImg(_event.ImageURL, _destiny);
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                    throw new Exception("Ocorreu um erro não especificado ao tentar deletar o evento");
                }
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
}
        #endregion
    }
}
