using AutoMapper;
using Application.Features.Users.Commands.Register;
using Domain.Entities;

namespace Application.Features.Users.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterCommand, UserAccount>()
         .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
         .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
         .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Nom))
         .ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => Guid.NewGuid()))
         .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}