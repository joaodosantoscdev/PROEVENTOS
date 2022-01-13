using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Repository.Interfaces
{
    public interface IPartRepository
    {
        // Parts

        /// <summary>
        /// Método Get que retornará uma lista de lotes por ID do Evento.
        /// </summary>
        /// <param name="eventId">Id do Evento</param>
        /// <returns>Lista[Array] de Lotes</returns>
        Task<Part[]> GetAllPartsByEventIdAsync(int eventId);

        /// <summary>
        /// Método get que retornará apenas 1 Lote.
        /// </summary>
        /// <param name="eventId">Id do Evento</param>
        /// <param name="Id">Id do Lote</param>
        /// <returns>1 Lote</returns>
        Task<Part> GetPartByIdsAsync(int eventId, int Id);
    }
}
