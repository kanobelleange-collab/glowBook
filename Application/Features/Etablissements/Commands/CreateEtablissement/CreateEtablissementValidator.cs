using System;
using System.Threading;

using FluentValidation;

namespace Application.Features.Etablissements.Commands.CreateEtablissement
{
    public class CreateEtablissementValidator : AbstractValidator<CreateEtablissementCommand>
    {
        public CreateEtablissementValidator()
        {
            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom de l'établissement est requis.")
                .MaximumLength(100).WithMessage("Le nom de l'établissement ne peut pas dépasser 100 caractères.");

            RuleFor(x => x.Adresse)
                .NotEmpty().WithMessage("L'adresse de l'établissement est requise.")
                .MaximumLength(200).WithMessage("L'adresse de l'établissement ne peut pas dépasser 200 caractères.");
            RuleFor(x=> x.Quartier)
                .NotEmpty().WithMessage("le quartier de letablissement ne peut pas depasser 30  caractere")
                .MaximumLength(30).WithMessage("le quartier de letablissement ne peut pas depasser 30  caractere");

            RuleFor(x => x.Ville)
                .NotEmpty().WithMessage("La ville de l'établissement est requise.")
                .MaximumLength(50).WithMessage("La ville de l'établissement ne peut pas dépasser 50 caractères.");

            RuleFor(x => x.Telephone)
                .NotEmpty().WithMessage("Le numéro de téléphone de l'établissement est requis.")
                .Matches(@"^\+?[0-9\s\-]+$").WithMessage("Le numéro de téléphone n'est pas valide.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email de l'établissement est requis.")
                .EmailAddress().WithMessage("L'email n'est pas valide.");

               }
    }
} 