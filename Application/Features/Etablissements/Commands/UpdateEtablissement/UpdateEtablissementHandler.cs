using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Etablissements.DTOs;
using Domain.Entities;
using Domain.Enum;
using Application.Features.Etablissements.Interfaces;
using Application.Features.Etablissements.Commands.UpdateEtablissement;
using AutoMapper;

namespace Application.Features.Etablissements.Commands.UpdateEtablissement
{
    public class UpdateEtablissementHandler : IRequestHandler<UpdateEtablissementCommand, EtablissementDto>
    {
        private readonly IEtablissementRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEtablissementHandler(IEtablissementRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EtablissementDto> Handle(UpdateEtablissementCommand request, CancellationToken cancellationToken)
        {
            // 1. Récupérer l'entité existante
            var etablissement = await _repository.GetByIdAsync(request.Id);
            if (etablissement == null)
                throw new Exception($"Établissement avec ID {request.Id} non trouvé.");

            // 2. Gérer le changement de type d'établissement si nécessaire
            if (!string.Equals(etablissement.GetType().Name, request.TypeEtablissement, StringComparison.OrdinalIgnoreCase))
            {
                Etablissement nouveauEtablissement = request.TypeEtablissement switch
                {
                    "SalonCoiffure" => new SalonCoiffure(
                        request.Nom, request.Adresse, request.Ville,
                        request.Telephone, request.Email,
                        request.SpecialitesTresse ?? new List<string>(),
                        request.TypesCheveux      ?? new List<string>(),
                        request.AccepteHommes,
                        request.AccepteEnfants),

                    "CabinetProthetiste" => new CabinetProthetiste(
                        request.Nom, request.Adresse, request.Ville,
                        request.Telephone, request.Email,
                        request.ProposeProtheseOngles,
                        request.ProposeExtensionCils,
                        request.ProposeProtheseCapillaire),

                    "SalonMassage" => new SalonMassage(
                        request.Nom, request.Adresse, request.Ville,
                        request.Telephone, request.Email,
                        request.TypesMassage       ?? new List<string>(),
                        request.Ambiance            ?? string.Empty,
                        request.DisponibleADomicile,
                        request.DureeMinimaleEnMinutes),

                    "SpaBeaute" => new SpaBeaute(
                        request.Nom, request.Adresse, request.Ville,
                        request.Telephone, request.Email,
                        request.Service),

                    _ => throw new ArgumentException($"Type d'établissement invalide : {request.TypeEtablissement}")
                };

                // Copier les photos et horaires existants
                foreach (var photo in etablissement.Photos)
                    nouveauEtablissement.AjouterPhoto(photo);

                foreach (var horaire in etablissement.Horaires)
                    nouveauEtablissement.AjouterHoraire(horaire);

                await _repository.UpdateAsync(nouveauEtablissement );
                return _mapper.Map<EtablissementDto>(nouveauEtablissement);
            }

            // 3. Mettre à jour les propriétés communes (type inchangé)
           
        etablissement.MettreAJour(
    request.Nom,
    request.Adresse,
    request.Ville,
    request.Telephone,
    request.Email
        , (CategorieEtablissement)Enum.Parse(typeof(CategorieEtablissement), request.TypeEtablissement)
        , request.Description
);

            // 4. Sauvegarder les modifications
            await _repository.UpdateAsync( etablissement );

            // 5. Retourner le DTO
            return _mapper.Map<EtablissementDto>(etablissement);
        }
    }
}