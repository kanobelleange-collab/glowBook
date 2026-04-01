using MediatR;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Commands.UpdateEtablissement
{
    public class UpdateEtablissementCommand : IRequest<EtablissementDto>
    {
        public Guid Id { get; set; }
        public required string Nom { get; set; }
        public required string Adresse { get; set; }
        public string Quartier { get; set; } = string.Empty;
        public required string Ville { get; set; }
        public required string Telephone { get; set; }
        public required string Email { get; set; }
        public string? Description { get; set; }
    }
}