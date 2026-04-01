using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Commands.AddServices.SalonMassage
{
    public class AddSalonMassageCommand : IRequest<Guid>
    {
        public Guid EtablissementId { get; set; }
        public List<string> TypesMassage { get; set; } = new();
        public string? Ambiance { get; set; }
        public bool DisponibleADomicile { get; set; } = false;
        public int DureeMinimaleEnMinutes { get; set; } = 30;
        public List<PrestationDto> Prestations { get; set; } = new();
    }
}