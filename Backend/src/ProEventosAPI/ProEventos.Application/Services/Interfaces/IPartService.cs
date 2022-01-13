using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Services.Interfaces
{
    public interface IPartService
    {
        Task<PartDTO[]> SaveParts(int eventId, PartDTO[] models);
        Task<bool> DeletePart(int eventId, int id);
        Task<PartDTO[]> GetAllPartsByEventIdAsync(int eventId);
        Task<PartDTO> GetPartByIdsAsync(int eventId, int id);
    }   
}