using AutoMapper;
using Domain.Entities;
using Application.Features.Employees.DTOs;

namespace Application.Features.Employees.Mapping
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            // Praticien → PraticienDto
            CreateMap<Employee, EmployeeDto>()
                
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