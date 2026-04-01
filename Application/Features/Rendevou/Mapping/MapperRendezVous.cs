using AutoMapper;
using Domain.Entities;
using Application.Features.Clients.Commands.CreateClient;
using Application.Features.Clients.Commands.UpdateClient;
using Application.Features.Rendevou.DTOs;

public class MappingRendezVous : Profile
{
    public MappingRendezVous()
    {

        CreateMap<RendezVous, RendezVousDto>().ReverseMap();;
    }
}