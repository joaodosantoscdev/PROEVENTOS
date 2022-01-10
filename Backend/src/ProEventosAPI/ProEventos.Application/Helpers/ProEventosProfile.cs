﻿using AutoMapper;
using ProEventos.Application.DTO;
using ProEventos.Domain.Models;

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
        }
    }
}