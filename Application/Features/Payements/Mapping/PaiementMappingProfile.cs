using AutoMapper;
using Domain.Entities;
using Application.Features.Payements.DTOs;

namespace Application.Features.Praticiens.Mapping
{
    public class PaiementMappingProfile : Profile
    {
        public PaiementMappingProfile()
        {
            // Praticien → PraticienDto
            CreateMap<Paiement, PaiementDto>()
    .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => src.Statut.ToString()));
}
    }
}