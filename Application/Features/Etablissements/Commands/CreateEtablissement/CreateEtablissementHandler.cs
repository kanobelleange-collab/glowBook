using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.Interfaces;
using Application.Features.Etablissements.DTOs;
using Application.Common.Interfaces; // ✅ IGeocodageService
using AutoMapper;
using Domain.Enum;

namespace Application.Features.Etablissements.Commands.CreateEtablissement
{
    public class CreateEtablissementHandler
        : IRequestHandler<CreateEtablissementCommand, EtablissementDto>
    {
        private readonly IEtablissementRepository _repository;
        private readonly IMapper                  _mapper;
        private readonly IGeocodageService        _geocodage; // ✅ ajouté

        public CreateEtablissementHandler(
            IEtablissementRepository repository,
            IMapper mapper,
            IGeocodageService geocodage) // ✅ ajouté
        {
            _repository = repository;
            _mapper     = mapper;
            _geocodage  = geocodage;
        }

        public async Task<EtablissementDto> Handle(
            CreateEtablissementCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Créer l'entité selon le type
            Etablissement etablissement = request.TypeEtablissement switch
            {
                "SalonCoiffure"      => CreerSalonCoiffure(request),
                "SalonMassage"       => CreerSalonMassage(request),
                "CabinetEsthetique"  => CreerCabinetEsthetique(request),
                "CabinetProthetiste" => CreerCabinetProthetiste(request),
                "SpaBeaute"          => CreerSpaBeaute(request),
                _ => throw new ArgumentException(
                    $"Type d'établissement invalide : {request.TypeEtablissement}")
            };

            // 2. ✅ Géocodage automatique — l'établissement entre juste ville + quartier
            var coords = await _geocodage.GeocodeAsync(
                request.Ville,
                request.Quartier); // ✅ ajoutez Quartier dans la Command

            if (coords != null)
                etablissement.DefinirLocalisation(
                    coords.Latitude,
                    coords.Longitude);

            // 3. Ajouter les photos si présentes
            if (request.Photos != null && request.Photos.Any())
                foreach (var photo in request.Photos)
                    etablissement.AjouterPhoto(photo);

            // 4. Ajouter les horaires si présents
            if (request.Horaires != null && request.Horaires.Any())
            {
                foreach (var horaireDto in request.Horaires)
                {
                    var jour      = Enum.Parse<DayOfWeek>(horaireDto.Jour, ignoreCase: true);
                    var ouverture = TimeSpan.Parse(horaireDto.HeureOuverture);
                    var fermeture = TimeSpan.Parse(horaireDto.HeureFermeture);
                    etablissement.AjouterHoraire(
                        new HoraireOuverture(jour, ouverture, fermeture));
                }
            }

            // 5. Sauvegarder en base
            await _repository.AddAsync(etablissement);

            // 6. Retourner le DTO
            return _mapper.Map<EtablissementDto>(etablissement);
        }

        // -------------------------------------------------------
        // Méthodes privées pour chaque type d'établissement
        // -------------------------------------------------------

        private SalonCoiffure CreerSalonCoiffure(CreateEtablissementCommand request)
        {
            if (request.SpecialitesTresse == null || !request.SpecialitesTresse.Any())
                throw new ArgumentException(
                    "Un salon de coiffure doit avoir au moins une spécialité de tresse.");

            if (request.TypesCheveux == null || !request.TypesCheveux.Any())
                throw new ArgumentException(
                    "Un salon de coiffure doit préciser les types de cheveux acceptés.");

            return new SalonCoiffure(
                request.Nom,
                request.Adresse,
                request.Ville,
                request.Telephone,
                request.Email,
                request.SpecialitesTresse,
                request.TypesCheveux,
                request.AccepteHommes,
                request.AccepteEnfants
            );
        }

        private SalonMassage CreerSalonMassage(CreateEtablissementCommand request)
        {
            if (request.TypesMassage == null || !request.TypesMassage.Any())
                throw new ArgumentException(
                    "Un salon de massage doit avoir au moins un type de massage.");

            if (string.IsNullOrWhiteSpace(request.Ambiance))
                throw new ArgumentException(
                    "Un salon de massage doit préciser son ambiance.");

            return new SalonMassage(
                request.Nom,
                request.Adresse,
                request.Ville,
                request.Telephone,
                request.Email,
                request.TypesMassage,
                request.Ambiance,
                request.DisponibleADomicile,
                request.DureeMinimaleEnMinutes
            );
        }

        private CabinetEsthetique CreerCabinetEsthetique(CreateEtablissementCommand request)
        {
            if (!request.ProposeSoinsVisage &&
                !request.ProposeEpilation   &&
                !request.ProposeOnglerie    &&
                !request.ProposeMaquillage)
                throw new ArgumentException(
                    "Un cabinet esthétique doit proposer au moins un type de soin.");

            return new CabinetEsthetique(
                request.Nom,
                request.Adresse,
                request.Ville,
                request.Telephone,
                request.Email,
                request.ProposeSoinsVisage,
                request.ProposeEpilation,
                request.ProposeOnglerie,
                request.ProposeMaquillage
            );
        }

        private CabinetProthetiste CreerCabinetProthetiste(CreateEtablissementCommand request)
        {
            if (!request.ProposeProtheseOngles    &&
                !request.ProposeExtensionCils     &&
                !request.ProposeProtheseCapillaire)
                throw new ArgumentException(
                    "Un cabinet prothésiste doit proposer au moins une prestation.");

            return new CabinetProthetiste(
                request.Nom,
                request.Adresse,
                request.Ville,
                request.Telephone,
                request.Email,
                request.ProposeProtheseOngles,
                request.ProposeExtensionCils,
                request.ProposeProtheseCapillaire
            );
        }

        private SpaBeaute CreerSpaBeaute(CreateEtablissementCommand request)
        {
            if (request.Service == 0)
                throw new ArgumentException(
                    "Un spa doit avoir au moins un équipement.");

            return new SpaBeaute(
                request.Nom,
                request.Adresse,
                request.Ville,
                request.Telephone,
                request.Email,
                request.Service
            );
        }
    }
}