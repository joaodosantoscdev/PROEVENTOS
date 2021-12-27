using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime? DateEvent { get; set; }
        public string Theme { get; set; }
        public int Capacity { get; set; }
        public string ImageURL { get; set; }
        public string CallNumber { get; set; }
        public int Email { get; set; }
        public IEnumerable<Part> Parts { get; set; }
        public IEnumerable<SocialMedia> SocialMedias { get; set; }
        public IEnumerable<SpeakerEvent> SpeakerEvents { get; set; }
    }
}
