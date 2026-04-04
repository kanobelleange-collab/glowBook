using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Employees.Queries.GetById
{
    public class GetEmployeeByIdQueryHandler :
        IRequestHandler<GetEmployeeByIdQuery, Employee?>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Employee?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.Id);
            if (employee == null) return null;

            // Mapper l'entité vers le DTO
            return _mapper.Map<Employee>(employee);
        }
    }
}