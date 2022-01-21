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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserService _userService;
        public EventController(IEventService eventService, 
                               IWebHostEnvironment hostEnvironment,
                               IUserService userService)
        {
            _userService = userService;
            _hostEnvironment = hostEnvironment;
            _eventService = eventService;
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
                    DeleteImg(_event.ImageURL);
                    _event.ImageURL = await SaveImg(file);
                }
                var eventReturn = await _eventService.UpdateEvent(User.GetUserId(), eventId, _event);

                return Ok(eventReturn);
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
                    DeleteImg(_event.ImageURL);
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

        // NON-ACTION METHODS - Events
        #region Non Action Methods - Controller
        [NonAction]
        public async Task<string> SaveImg(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray())
                .Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow:yymmssfff}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);

            using (var FileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(FileStream);
            }

            return imageName;
        }

        [NonAction]
        public void DeleteImg(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

        }
        #endregion
    }
}
