using System;
using FluentValidation;



namespace Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom est obligatoire.")
                .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email est obligatoire.")
                .EmailAddress().WithMessage("L'email doit être valide.");

            RuleFor(x => x.Telephone)
                .NotEmpty().WithMessage("Le téléphone est obligatoire.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Le téléphone doit être un numéro valide.");

            RuleFor(x => x.Ville)
                .NotEmpty().WithMessage("La ville est obligatoire.")
                .MaximumLength(100).WithMessage("La ville ne peut pas dépasser 100 caractères.");
        }
    }
}

