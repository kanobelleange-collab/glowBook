using System;
using FluentValidation;



namespace Application.Features.Rendevou.Commands.AnnulerRendeVous
{
    public class AnnulerRendeVousValidator : AbstractValidator<AnnulerRendeVousCommand>
    {
        public AnnulerRendeVousValidator()
        {
            RuleFor(x => x.RendezVousId).NotEmpty().WithMessage("L'ID du rendez-vous est requis.");
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("L'ID du client est requis.");
            RuleFor(x => x.Raison).MaximumLength(500).WithMessage("La raison d'annulation ne peut pas dépasser 500 caractères.");
        }
    }
}