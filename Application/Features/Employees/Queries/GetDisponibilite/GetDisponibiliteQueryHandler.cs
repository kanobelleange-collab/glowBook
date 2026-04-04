using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Interfaces;
using Application.Features.Employees.Queries;
using AutoMapper;

namespace Application.Features.Employees.Queries.GetDisponibilite
{
    public class GetDisponibiliteEmployeeQueriesHandler :
        IRequestHandler<GetAvailableEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

public GetDisponibiliteEmployeeQueriesHandler(IEmployeeRepository repository, IMapper mapper)
{
    _repository = repository;
    _mapper = mapper;
}

public async Task<List<EmployeeDto>> Handle(GetAvailableEmployeesQuery request, CancellationToken cancellationToken)
{
    var employees = await _repository.GetDisponiblesAsync(request.EtablissementId, request.DateHeure);
    return _mapper.Map<List<EmployeeDto>>(employees);
}
    }
}