using AutoMapper;
using Domain.Entities;
using Application.Features.ServiceEsthetiques.DTOs;

namespace Application.Features.ServiceEsthetiques.Mapping
{
    public class ServiceEsthetiqueMappingProfile : Profile
    {
        public ServiceEsthetiqueMappingProfile()
        {
            // ServiceEsthetique → ServiceEsthetiqueDto
            CreateMap<ServiceEsthetique, ServiceEsthetiqueDto>()
                .ForMember(dest => dest.DureeEnMinutes,
                    opt => opt.MapFrom(src => src.DureeFormatee));
            // DureeFormatee est une propriété calculée de l'entité
            // AutoMapper la mappe automatiquement

            // CreerServiceEsthetiqueDto → ServiceEsthetique
            // On n'utilise pas ce mapping directement car le constructeur
            // de ServiceEsthetique prend des paramètres — on le fait manuellement
        }
    }
}