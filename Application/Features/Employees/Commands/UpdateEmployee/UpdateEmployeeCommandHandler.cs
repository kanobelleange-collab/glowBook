using MediatR;
using Application.Features.Employees.Commands;
using Application.Features.Employees.Interfaces;
using Domain.Entities;
using Application.Features.Employees.DTOs;
using AutoMapper;

namespace Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.Id);
            if (employee is null) 
                throw new Exception("Employé introuvable");

            // Mettre à jour les champs
            employee.Nom = request.Nom;
            employee.Prenom = request.Prenom;
            employee.Specialite = request.Specialite;
            employee.UrlPhoto = request.UrlPhoto;
            employee.AnneesExperience = request.AnneesExperience;
            

            await _repository.UpdateAsync(employee);

            // Mapper l'entité en DTO
            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}