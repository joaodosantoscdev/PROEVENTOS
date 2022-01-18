using ProEventos.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Domain.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string CV { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<SocialMedia> SocialMedias { get; set; }
        public IEnumerable<SpeakerEvent> SpeakerEvents { get; set; }
    }
}
