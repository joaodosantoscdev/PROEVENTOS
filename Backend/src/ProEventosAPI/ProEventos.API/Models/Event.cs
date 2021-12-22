using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventosAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime DateEvent { get; set; }
        public string Theme { get; set; }
        public int Capacity { get; set; }
        public string Part { get; set; }
        public string ImageURL { get; set; }
    }
}
