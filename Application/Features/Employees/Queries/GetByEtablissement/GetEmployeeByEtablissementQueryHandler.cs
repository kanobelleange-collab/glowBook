using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Interfaces;
using AutoMapper;

namespace Application.Features.Employees.Queries.GetByEtablissement
{
    public class GetEmployeesByEtablissementQueryHandler :
        IRequestHandler<GetEmployeesByEtablissementQuery, List<EmployeeDto>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeesByEtablissementQueryHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeesByEtablissementQuery request, CancellationToken cancellationToken)
        {
            var employees = await _repository.GetByEtablissementAsync(request.EtablissementId);
            return _mapper.Map<List<EmployeeDto>>(employees);
        }
    }
}