using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.DTO;

namespace ProEventosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly IPartService _partService;
        public PartController(IPartService partService)
        {
            _partService = partService;
        }
        #endregion

        // GET - Part
        #region GET Methods - Part Controller
        [HttpGet("{eventId}")]
        public async Task<IActionResult> Get(int eventId)
        {
            try
            {
                var _parts = await _partService.GetAllPartsByEventIdAsync(eventId);
                if (_parts == null) return NoContent();

                return Ok(_parts);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar os lotes. Erro: {e.Message}");
            }
        }
        #endregion

        // UPDATE - Part
        #region UPDATE Methods - Part Controller
        [HttpPut("{eventId}")]
        public async Task<IActionResult> SaveLotes(int eventId, PartDTO[] models)
        {
            try
            {
                var _parts = await _partService.SaveParts(eventId, models);
                if (_parts == null) return NoContent();

                return Ok(_parts);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar os lotes. Erro {e.Message}");
            }
        }
        #endregion

        // DELETE - Part
        #region DELETE Methods - Part Controller
        [HttpDelete("{eventId}/{id}")]
        public async Task<IActionResult> Delete(int eventId, int id)
        {
            try
            {
                var _part = await _partService.GetPartByIdsAsync(eventId, id);
                if (_part == null) return NoContent();

                return await _partService.DeletePart(eventId, id) 
                    ? Ok( new { message = "Lote deletado" })
                    : throw new Exception("Ocorreu um erro não especificado ao tentar deletar o lote");
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar encontrar os lotes. Erro {e.Message}");
            }
        }
        #endregion
    }
}
