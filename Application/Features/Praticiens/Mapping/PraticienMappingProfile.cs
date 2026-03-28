using AutoMapper;
using Domain.Entities;
using Application.Features.Praticiens.DTOs;

namespace Application.Features.Praticiens.Mapping
{
    public class PraticienMappingProfile : Profile
    {
        public PraticienMappingProfile()
        {
            // Praticien → PraticienDto
            CreateMap<Praticien, PraticienDto>()
                .ForMember(dest => dest.NomComplet,
                    opt => opt.MapFrom(src => src.NomComplet))
                .ForMember(dest => dest.Disponibilites,
                    opt => opt.MapFrom(src => src.Disponibilites)).ReverseMap();

            // Disponibilite → DisponibiliteDto
            CreateMap<Disponibilite, DisponibiliteDto>()
                .ForMember(dest => dest.Jour,
                    opt => opt.MapFrom(src => src.Jour.ToString()))
                .ForMember(dest => dest.HeureDebut,
                    opt => opt.MapFrom(src => src.HeureDebut.ToString(@"hh\:mm")))
                .ForMember(dest => dest.HeureFin,
                    opt => opt.MapFrom(src => src.HeureFin.ToString(@"hh\:mm"))).ReverseMap();
                    
        }
    }
}