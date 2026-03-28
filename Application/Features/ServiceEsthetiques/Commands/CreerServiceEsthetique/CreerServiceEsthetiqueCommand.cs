using System;
using MediatR;
using Application.Features.ServiceEsthetiques.DTOs;

namespace Application.Features.ServiceEsthetiques.Commands.CreerServiceEsthetique
{
    public class CreerServiceEsthetiqueCommand : IRequest<ServiceEsthetiqueDto>
    {
        public required string Nom { get; set; }
        public required string Description { get; set; }
        public required decimal Prix { get; set; }
        public  required int DureeEnMinutes { get; set; }
        public required string Categorie { get; set; }
        public  Guid EtablissementId { get; set; }
    }
}