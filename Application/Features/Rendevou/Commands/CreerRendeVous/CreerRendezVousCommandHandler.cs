using MediatR;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Employees.Interfaces;
using Application.Features.Etablissements.Interfaces;
using Application.Features.Prestations.Interfaces;
using Application.Features.Notifications.Interfaces;
using Application.Features.Rendevou.DTOs;
using Domain.Entities;
using RdvEntity = Domain.Entities.RendezVous; // alias pour new RdvEntity(...)

// ❌ Supprimer cette ligne — elle est inutile car on est déjà dans ce namespace
// using Application.Features.RendeVous.Commands.CreerRendeVous;

namespace Application.Features.Rendevou.Commands.CreerRendeVous
{
    public class CreerRendezVousCommandHandler
        : IRequestHandler<CreerRendeVousCommand, RendezVousDto>
    {
        private readonly IRendezVousRepository _rdvRepository;
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IEtablissementRepository _etablissementRepository;
        private readonly IPrestationRepository _prestationRepository;
        private readonly INotificationService _notificationService;

        public CreerRendezVousCommandHandler(
            IRendezVousRepository rdvRepository,
            IEmployeeRepository employeeRepository,
            IEtablissementRepository etablissementRepository,
            IPrestationRepository prestationRepository,
            INotificationService notificationService)
        {
            _rdvRepository       = rdvRepository;
            _EmployeeRepository = employeeRepository;
            _etablissementRepository   = etablissementRepository;
            _prestationRepository   = prestationRepository;
            _notificationService = notificationService;
        }

        public async Task<RendezVousDto> Handle(
            CreerRendeVousCommand command,
            CancellationToken cancellationToken)
        {
            var employee = await _EmployeeRepository.GetByIdAsync(command.EmployeeId)
                ?? throw new Exception("Employer introuvable.");

            bool occupe = await _rdvRepository
                .CreneauDejaOccupeAsync(command.EmployeeId, command.DateHeure);
            if (occupe)
                throw new Exception("Ce créneau est déjà occupé.");

            // var service = await _prestationRepository.GetByIdAsync(command.ServiceId)
            //     ?? throw new Exception("Service introuvable.");
                var service = await _etablissementRepository.GetServiceByIdAsync(command.ServiceId)
                ?? throw new Exception("Service introuvable.");

            var rdv = new RdvEntity(
                command.ClientId,
                command.EmployeeId,
                command.ServiceId,
                command.EtablissementId,
                command.DateHeure,
                command.Prix
            
            );

            // ✅ rdv est bien Domain.Entities.RendezVous grâce à l'alias RdvEntity
            await _rdvRepository.AddAsync(rdv);

            await _notificationService
                .EnvoyerConfirmationRdvAsync(command.ClientId, rdv.Id);

            return new RendezVousDto
            {
                Id              = rdv.Id,
                ClientId        = rdv.ClientId,
                EmployeeId     = rdv.EmployeeId,
                ServiceId       = rdv.ServiceId,
                EtablissementId = rdv.EtablissementId,
                DateHeure       = rdv.DateHeure,
                Prix            = rdv.Prix,
                Statut          = rdv.Statut
               
            };
        }
    }
}