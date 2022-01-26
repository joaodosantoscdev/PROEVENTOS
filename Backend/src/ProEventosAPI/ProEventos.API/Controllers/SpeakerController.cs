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
    public class SpeakerController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly ISpeakerService _speakerService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserService _userService;
        public SpeakerController(ISpeakerService speakerService, 
                                 IWebHostEnvironment hostEnvironment,
                                 IUserService userService)
        {
            _userService = userService;
            _hostEnvironment = hostEnvironment;
            _speakerService = speakerService;
        }
        #endregion

        // GET - Speakers
        #region GET Methods - Controller
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pgParams)
        {
            try
            {
                var _speakers = await _speakerService.GetAllSpeakersAsync(pgParams, true);
                if (_speakers == null) return NoContent();

                Response.AddPagination(_speakers.CurrentPage, _speakers.PageSize, _speakers.TotalCount, _speakers.TotalPages);

                return Ok(_speakers);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar palestrantes. Erro: {e.Message}");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetSpeakers()
        {
            try
            {
                var _speakers = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId(), true);
                if (_speakers == null) return NoContent();

                return Ok(_speakers);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar palestrantes. Erro {e.Message}");
            }
        }
        #endregion

        // ADD - Speakers
        #region POST Methods - Controller
        [HttpPost("")]
        public async Task<IActionResult> Add(SpeakerAddDTO model)
        {
            try
            {
                var _speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId(), false);
                if (_speaker == null)
                    _speaker = await _speakerService.AddSpeakers(User.GetUserId(), model);

                return Ok(_speaker);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }
        #endregion

        //UPDATE - Speakers
        #region UPDATE Methods - Controller
        [HttpPut]
        public async Task<IActionResult> Put(SpeakerUpdateDTO model)
        {
            try
            {
                var _speaker = await _speakerService.UpdateSpeaker(User.GetUserId(), model);
                if (_speaker == null) return NoContent();

                return Ok(_speaker);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar evento. Erro {e.Message}");
            }
        }
        #endregion
    }
}
