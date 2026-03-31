using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.Interfaces;
using Application.Features.Etablissements.DTOs;
using Application.Common.Interfaces;
using AutoMapper;

namespace Application.Features.Etablissements.Commands.CreateEtablissement
{
    public class CreateEtablissementHandler
        : IRequestHandler<CreateEtablissementCommand, EtablissementDto>
    {
        private readonly IEtablissementRepository _repository;
        private readonly IMapper                  _mapper;
        private readonly IGeocodageService        _geocodage;

        public CreateEtablissementHandler(
            IEtablissementRepository repository,
            IMapper mapper,
            IGeocodageService geocodage)
        {
            _repository = repository;
            _mapper     = mapper;
            _geocodage  = geocodage;
        }

        public async Task<EtablissementDto> Handle(
            CreateEtablissementCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Créer l'établissement de base
            var etablissement = new Etablissement(
                request.Nom,
                request.Adresse,
                request.Ville,
                request.Telephone,
                request.Email,
                request.Description
            );

            etablissement.DefinirQuartier(request.Quartier);

            // 2. Géocodage automatique
            var coords = await _geocodage.GeocodeAsync(request.Ville, request.Quartier);
            if (coords != null)
                etablissement.DefinirLocalisation(coords.Latitude, coords.Longitude);

            // 3. Photos
            if (request.Photos != null && request.Photos.Any())
                foreach (var photo in request.Photos)
                    etablissement.AjouterPhoto(photo);

            // 4. Horaires
            if (request.Horaires != null && request.Horaires.Any())
                foreach (var h in request.Horaires)
                {
                    var jour = HoraireOuverture.ConvertirJour(h.Jour);
                    var ouverture = HoraireOuverture.ParseHeure(h.HeureOuverture);
                    var fermeture = HoraireOuverture.ParseHeure(h.HeureFermeture);
                    etablissement.AjouterHoraire(new HoraireOuverture(jour, ouverture, fermeture));
                }

            // 5. Sauvegarder
            await _repository.AddAsync(etablissement);

            // 6. Retourner le DTO avec l'Id pour la suite
            return _mapper.Map<EtablissementDto>(etablissement);
        }
    }
}