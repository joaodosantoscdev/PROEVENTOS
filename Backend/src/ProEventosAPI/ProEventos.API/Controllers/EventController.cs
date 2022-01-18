using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.DTO;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace ProEventosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly IEventService _eventService;
        private readonly IWebHostEnvironment _hostEnvironment;
        public EventController(IEventService eventService, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
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
                if (_events == null) return NoContent();

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
                var _event = await _eventService.GetEventByIdAsync(id, true);
                if (_event == null) return NoContent();

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
                if (_event == null) return NoContent();

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
        public async Task<IActionResult> Add(EventDTO model)
        {
            try
            {
                var _event = await _eventService.AddEvents(model);
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
                var _event = await _eventService.GetEventByIdAsync(eventId, true);
                if (_event == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    DeleteImg(_event.ImageURL);
                    _event.ImageURL = await SaveImg(file);
                }
                var eventReturn = await _eventService.UpdateEvent(eventId, _event);

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
                var _event = await _eventService.UpdateEvent(id, model);
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
                return await _eventService.DeleteEvent(id) 
                    ? Ok( new { message = "Deletado" })
                    : throw new Exception("Ocorreu um erro não especificado ao tentar deletar o evento");
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

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

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
