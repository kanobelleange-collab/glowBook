using AutoMapper;
using Domain.Entities;
using Application.Features.Prestations.DTOs;
using Application.Features.Prestations.Commands.CreatePrestation;

namespace Application.Features.Prestations.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
         CreateMap<Prestation, PrestationDto>().ReverseMap();
         CreateMap<CreatePrestationCommand, Prestation>()
    .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Nom));
        }
    }
}
