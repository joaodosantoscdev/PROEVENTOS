using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Domain
{
    public class SpeakerEvent
    {
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
