using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.DTO
{
    public class SpeakerDTO
    {
        public int Id { get; set; }
        public string CV { get; set; }
        public int UserId { get; set; }
        public UserUpdateDTO User { get; set; }
        public IEnumerable<SocialMediaDTO> SocialMedias { get; set; }
        public IEnumerable<EventDTO> Events { get; set; }
    }
}
