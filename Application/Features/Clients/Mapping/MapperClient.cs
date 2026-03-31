using AutoMapper;
using Domain.Entities;
using Application.Features.Clients.Commands.CreateClient;
using Application.Features.Clients.Commands.UpdateClient;
using Application.Features.Clients.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateClientCommand, Client>().ReverseMap();
        CreateMap<UpdateClientCommand, Client>();
        CreateMap<Client, ClientDto>().ReverseMap();;
    }
}