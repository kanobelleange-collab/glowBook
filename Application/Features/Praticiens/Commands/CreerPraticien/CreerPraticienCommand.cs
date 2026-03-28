using MediatR;
using Application.Features.Praticiens.DTOs;

namespace Application.Features.Praticiens.Commands
{
    public class CreerPraticienCommand : IRequest<PraticienDto>
    {
        public  required string Nom { get; set; }
        public  required string Prenom { get; set; }
        public  required  string Specialite { get; set; }
        public string? Description { get; set; }
        public  required int AnneesExperience { get; set; }
        public Guid EtablissementId { get; set; }
    }
}