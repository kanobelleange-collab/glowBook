using AutoMapper;
using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Interfaces;
using Domain.Entities;
using Application.Features.Employees.Commands.AjoutDisponibilite;

namespace Application.Features.Employees.Commands
{
    public class AjouterDisponibiliteCommandHandler
        : IRequestHandler<AjouterDisponibiliteCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IMapper _mapper;

        public AjouterDisponibiliteCommandHandler(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _EmployeeRepository = employeeRepository;
            _mapper              = mapper;
        }

       public async Task<EmployeeDto> Handle(
    AjouterDisponibiliteCommand command,
    CancellationToken cancellationToken)
{
    // 1. Récupérer l'employé (incluant ses dispos actuelles via le nouveau GetByIdAsync)
    var employee = await _EmployeeRepository.GetByIdAsync(command.EmployeeId);
    
    if (employee == null)
        throw new Exception($"Employee avec l'Id {command.EmployeeId} introuvable.");

    // 2. Utilisation de vos méthodes de parsing personnalisées
    var jour = Disponibilite.ConvertirJour(command.Jour);
    var heureDebut = Disponibilite.ParseHeure(command.HeureDebut);
    var heureFin = Disponibilite.ParseHeure(command.HeureFin);

    // 3. Créer et ajouter la disponibilité
    var disponibilite = new Disponibilite(jour, heureDebut, heureFin);
// UTILISEZ :
employee.AjouterDisponibilite(disponibilite);

    // 4. Sauvegarder (via la méthode transactionnelle du Repository)
    await _EmployeeRepository.UpdateAsync(employee);

    // 5. Retourner le DTO mis à jour
    return _mapper.Map<EmployeeDto>(employee);
}
    }
}