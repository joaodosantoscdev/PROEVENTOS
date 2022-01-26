using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.DTO
{
    public class SpeakerUpdateDTO
    {
        public int Id { get; set; }
        public string CV { get; set; }
        public int UserId { get; set; }
    }
}
