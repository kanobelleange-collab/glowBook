using Domain.Entities;
using Application.Features.Prestations.DTOs;
using MediatR;

namespace Application.Features.Prestations.Commands.CreatePrestation
{
    public class CreatePrestationCommand : IRequest<PrestationDto>
{
     public Guid ServiceId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Prix { get; set; }

     public int DureeMinutes { get;  set; }
}
}