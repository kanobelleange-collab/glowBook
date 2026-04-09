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
            _mapper              = mapper;
        }

        public async Task<EmployeeDto> Handle(
            CreerEmployeeCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Créer l'entité
            var employee = new Employee(
                
                command.EtablissementId,
                command.Nom,

                command.Prenom,
                command.Specialite,
                command.UrlPhoto,
               
                command.AnneeExperience
                
            );

          // 2. Ajouter la description si présente

            // 3. Sauvegarder en base
            await _EmployeeRepository.AddAsync(employee);

            // 4. Mapper et retourner
            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}