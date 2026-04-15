using AutoMapper;
using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Interfaces;
using Domain.Entities;

namespace Application.Features.Employees.Commands.CreerEmployee
{
    public class CreerEmployeeCommandHandler
        : IRequestHandler<CreerEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IMapper _mapper;

        public CreerEmployeeCommandHandler(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _EmployeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(
            CreerEmployeeCommand command,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command.Nom, nameof(command.Nom));
            ArgumentNullException.ThrowIfNull(command.Prenom, nameof(command.Prenom));
            ArgumentNullException.ThrowIfNull(command.Specialite, nameof(command.Specialite));

            // 1. Créer l'entité
            var employee = new Employee(
                command.EtablissementId,
                command.Nom,
                command.Prenom,
                command.Specialite,
                string.Empty,
                command.AnneeExperience
            )
            {
                UrlPhoto = command.UrlPhoto
            };

            // 2. Sauvegarder en base
            await _EmployeeRepository.AddAsync(employee);

            // 3. Mapper et retourner
            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}