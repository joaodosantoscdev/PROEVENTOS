using AutoMapper;
using ProEventos.Application.DTO;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<Part, PartDTO>().ReverseMap();
            CreateMap<SocialMedia, SocialMediaDTO>().ReverseMap();

            CreateMap<Speaker, SpeakerDTO>().ReverseMap();
            CreateMap<Speaker, SpeakerAddDTO>().ReverseMap();
            CreateMap<Speaker, SpeakerUpdateDTO>().ReverseMap();



            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
        }
    }
}
