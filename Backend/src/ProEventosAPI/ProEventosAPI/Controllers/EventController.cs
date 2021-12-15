using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventosAPI.Data;
using ProEventosAPI.Models;
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
        private readonly DataContext _context;
        public EventController(DataContext context)
        {
            _context = context;
        }
        #endregion

        // GET - Events
        #region GET Methods
        [HttpGet]
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
    }
}
