using ProEventos.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using ProEventos.Persistence.Repository.Interfaces;
using ProEventos.Application.DTO;
using ProEventos.Domain.Models;
using AutoMapper;
using System.Linq;

namespace ProEventos.Application.Services
{
    public class PartSevice : IPartService
    {
        // Constructor &  Dependencies
        #region DI Injected 
        private readonly IGeneralRepository _generalRepository;
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;

        public PartSevice(IGeneralRepository generalRepository,
                          IPartRepository partRepository, 
                          IMapper mapper)
        {
            _mapper = mapper;
            _generalRepository = generalRepository;
            _partRepository = partRepository;
        }
        #endregion

        // ADD - Parts
        #region POST Methods - Services
        public async Task AddPart(int eventId, PartDTO model)
        {
            try 
            {
                var _part = _mapper.Map<Part>(model);
                _part.EventId = eventId;

                _generalRepository.Add(_part);

                await _generalRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // UPDATE - Parts
        #region UPDATE Methods - Part Services
        public async Task<PartDTO[]> SaveParts(int eventId, PartDTO[] models)
        {
            try
            {
                var _parts = await _partRepository.GetAllPartsByEventIdAsync(eventId);
                if (_parts == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddPart(eventId, model);
                    } 
                    else
                    {
                        var part = _parts.FirstOrDefault(part => part.Id == model.Id);
                        model.EventId = eventId;

                        _mapper.Map(model, part);

                        _generalRepository.Update(part);

                        await _generalRepository.SaveChangesAsync();
                    }
                }

                var partResult = await _partRepository.GetAllPartsByEventIdAsync(eventId);

                return _mapper.Map<PartDTO[]>(partResult);
            }
            catch (Exception e)
            {
                throw new Exception (e.Message);
            }
        }
        #endregion

        // DELETE - Parts
        #region DELETE Methods - Part Services
        public async Task<bool> DeletePart(int eventId, int id)
        {
            try
            {
                var _part = await _partRepository.GetPartByIdsAsync(eventId, id);
                if (_part == null) throw new Exception ("Não foi possível realizar essa ação, Lote não encontrado!");

                _generalRepository.Delete(_part);

                return await _generalRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        // GET - Parts
        #region GET Methods - Part Services
        public async Task<PartDTO[]> GetAllPartsByEventIdAsync(int eventId)
        {
            try
            {
                var _parts = await _partRepository.GetAllPartsByEventIdAsync(eventId);
                if (_parts == null) return null;

                var result = _mapper.Map<PartDTO[]>(_parts);

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<PartDTO> GetPartByIdsAsync(int eventId, int id)
        {
            try
            {
                var _parts = await _partRepository.GetPartByIdsAsync(eventId, id);
                if (_parts == null) return null;

                var result = _mapper.Map<PartDTO>(_parts);

                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
