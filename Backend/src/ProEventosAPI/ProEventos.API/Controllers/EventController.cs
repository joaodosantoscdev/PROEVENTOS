using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly ProEventosContext _context;
        public EventController(ProEventosContext context)
        {
            _context = context;
        }
        #endregion

        // GET - Events
        #region GET Methods
        [HttpGet("")]
        public IEnumerable<Event> Get()
        {
            return _context.Events;
        }

        [HttpGet("{id}")]
        public Event GetById(int id)
        {
            return _context.Events.FirstOrDefault(e => e.Id == id);
        }
        #endregion

        // ADD - Events
        #region ADD Methods
        [HttpPost("")]
        public void Add(Event _event) 
        {
            _event.DateEvent = DateTime.Now;

            _context.Events.Add(_event);
            _context.SaveChanges();
        } 
        #endregion
    }
}
