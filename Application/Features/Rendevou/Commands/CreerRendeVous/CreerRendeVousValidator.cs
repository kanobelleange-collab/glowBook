using System;
using FluentValidation;
using Domain.Entities;
using Application.Features.Rendevou.Commands.CreerRendeVous;

namespace Application.Features.Rendevou.Commands.CreerRendeVous
{
    public class CreerRendeVousValidator : AbstractValidator<CreerRendeVousCommand>
    {
        public CreerRendeVousValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("L'ID du client est requis.");
            RuleFor(x => x.PraticienId).NotEmpty().WithMessage("L'ID du praticien est requis.");
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("L'ID du service est requis.");
            RuleFor(x => x.EtablissementId).NotEmpty().WithMessage("L'ID de l'établissement est requis.");
            RuleFor(x => x.DateHeure).GreaterThan(DateTime.Now).WithMessage("La date et l'heure doivent être dans le futur.");
            RuleFor(x => x.NotesClient).MaximumLength(500).WithMessage("Les notes du client ne peuvent pas dépasser 500 caractères.");
        }
    }
}