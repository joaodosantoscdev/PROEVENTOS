using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.DTO
{
    public class SocialMediaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int? EventId { get; set; }
        public EventDTO Event { get; set; }
        public int? SpeakerId { get; set; }
        public SpeakerDTO Speaker { get; set; }
    }
}
