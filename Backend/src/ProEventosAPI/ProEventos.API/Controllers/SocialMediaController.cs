using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using ProEventos.API.Extensions;

namespace ProEventosAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SocialMediaController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly ISocialMediaService _socialMediaService;
        private readonly IEventService _eventService;
        private readonly ISpeakerService _speakerService;

        public SocialMediaController(ISocialMediaService socialMediaService,
                                     IEventService eventService,
                                     ISpeakerService speakerService)
        {
            _socialMediaService = socialMediaService;
            _eventService = eventService;
            _speakerService = speakerService;
        }
        #endregion

        // GET - Social Media
        #region GET Methods - Social Media Controller
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            try
            {
                if (!(await EventAuthor(eventId)))
                    return Unauthorized();

                var _socialMedias = await _socialMediaService.GetAllByEventIdAsync(eventId);
                if (_socialMedias == null) return NoContent();

                return Ok(_socialMedias);
            }
            catch (Exception e)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar as redes sociais por evento. Erro: {e.Message}"
                    );
            }
        }

        [HttpGet("speaker")]
        public async Task<IActionResult> GetBySpeaker()
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId());
                if (speaker == null) return Unauthorized();

                var _socialMedias = await _socialMediaService.GetAllBySpeakerIdAsync(speaker.Id);
                if (_socialMedias == null) return NoContent();

                return Ok(_socialMedias);
            }
            catch (Exception e)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar as redes sociais por palestrante. Erro: {e.Message}"
                    );
            }
        }
        #endregion

        // UPDATE - Social Media
        #region UPDATE Methods - Social Media Controller
        [HttpPut("event/{eventId}")]
        public async Task<IActionResult> SaveByEvent(int eventId, SocialMediaDTO[] models)
        {
            try
            {
                if (!(await EventAuthor(eventId)))
                    return Unauthorized();

                var _socialMedia = await _socialMediaService.SaveByEvent(eventId, models);
                if (_socialMedia == null) return NoContent();

                return Ok(_socialMedia);
            }
            catch (Exception e)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar as redes sociais por evento. Erro {e.Message}"
                    );
            }
        }

        [HttpPut("speakerId")]
        public async Task<IActionResult> SaveBySpeaker(SocialMediaDTO[] models)
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId());
                if (speaker == null) return Unauthorized();

                var _socialMedia = await _socialMediaService.SaveBySpeaker(speaker.Id, models);
                if (_socialMedia == null) return NoContent();

                return Ok(_socialMedia);
            }
            catch (Exception e)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar as redes sociais por palestrante. Erro {e.Message}"
                    );
            }
        }
        #endregion

        // DELETE - Social Media
        #region DELETE Methods - Social Media Controller
        [HttpDelete("event/{eventId}/{socialMediaId}")]
        public async Task<IActionResult> DeleteByEvent(int eventId, int socialMediaId)
        {
            try
            {
                if (!(await EventAuthor(eventId)))
                    return Unauthorized();

                var _socialMedia = await _socialMediaService.GetEventSocialMediaByIdsAsync(eventId, socialMediaId);
                if (_socialMedia == null) return NoContent();

                return await _socialMediaService.DeleteByEvent(eventId, socialMediaId) 
                    ? Ok( new { message = "Rede Social deletada." })
                    : throw new Exception("Ocorreu um erro não especificado ao tentar deletar o rede social por evento.");
            }
            catch (Exception e)
            {

                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar excluir a rede social associada ao palestrante. Erro {e.Message}"
                    );
            }
        }

        [HttpDelete("speaker/{socialMediaId}")]
        public async Task<IActionResult> DeleteBySpeaker(int socialMediaId)
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId());
                if (speaker == null) return Unauthorized();

                var _socialMedia = await _socialMediaService.GetSpeakerSocialMediaByIdsAsync(speaker.Id, socialMediaId);
                if (_socialMedia == null) return NoContent();

                return await _socialMediaService.DeleteBySpeaker(speaker.Id, socialMediaId)
                    ? Ok(new { message = "Rede Social deletada." })
                    : throw new Exception("Ocorreu um erro não especificado ao tentar deletar o rede social por palestrante.");
            }
            catch (Exception e)
            {

                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar excluir a rede social associada ao palestrante. Erro {e.Message}"
                    );
            }
        }
        #endregion

        // NON-ACTION
        #region NON ACTION Methods - Social Media
        [NonAction]
        private async Task<bool> EventAuthor(int eventId)
        {
            var _event = await _eventService.GetEventByIdAsync(User.GetUserId(), eventId, false);
            if (_event == null) return false;

            return true;
        }
        #endregion
    }
}
