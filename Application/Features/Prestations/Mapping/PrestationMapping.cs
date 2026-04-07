using AutoMapper;
using Domain.Entities;
using Application.Features.Prestations.DTOs;

namespace Application.Features.Prestations.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
         CreateMap<Prestation, PrestationDto>().ReverseMap();
        }
    }
}
