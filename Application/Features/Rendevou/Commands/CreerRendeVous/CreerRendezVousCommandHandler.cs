using MediatR;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Employees.Interfaces;
using Application.Features.ServiceEsthtiques.Interfaces;
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
        private readonly IServiceEsthetiqueRepository _serviceRepository;
        private readonly INotificationService _notificationService;

        public CreerRendezVousCommandHandler(
            IRendezVousRepository rdvRepository,
            IEmployeeRepository employeeRepository,
            IServiceEsthetiqueRepository serviceRepository,
            INotificationService notificationService)
        {
            _rdvRepository       = rdvRepository;
            _EmployeeRepository = employeeRepository;
            _serviceRepository   = serviceRepository;
            _notificationService = notificationService;
        }

        public async Task<RendezVousDto> Handle(
            CreerRendeVousCommand command,
            CancellationToken cancellationToken)
        {
            var employee = await _EmployeeRepository.GetByIdAsync(command.EmployeeId)
                ?? throw new Exception("Praticien introuvable.");

            bool occupe = await _rdvRepository
                .CreneauDejaOccupeAsync(command.EmployeeId, command.DateHeure);
            if (occupe)
                throw new Exception("Ce créneau est déjà occupé.");

            var service = await _serviceRepository.GetByIdAsync(command.ServiceId)
                ?? throw new Exception("Service introuvable.");

            var rdv = new RdvEntity(
                command.ClientId,
                command.EmployeeId,
                command.ServiceId,
                command.EtablissementId,
                command.DateHeure,
                service.Prix
            
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