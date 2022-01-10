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
        public string Name { get; set; }
        public string CV { get; set; }
        public string ImageURL { get; set; }
        public string CallNumber { get; set; }
        public string Email { get; set; }
        public IEnumerable<SocialMediaDTO> SocialMedias { get; set; }
        public IEnumerable<SpeakerDTO> Speakers { get; set; }
    }
}
