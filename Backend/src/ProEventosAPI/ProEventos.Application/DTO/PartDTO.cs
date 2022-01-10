using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.DTO
{
    public class PartDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string DateInitial { get; set; }
        public string DateEnd { get; set; }
        public int Quantity { get; set; }
        public int EventId { get; set; }
        public EventDTO Event { get; set; }
    }
}
