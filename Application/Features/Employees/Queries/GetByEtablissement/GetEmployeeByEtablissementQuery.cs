using MediatR;
using Application.Features.Employees.DTOs;
using System;
using System.Collections.Generic;

namespace Application.Features.Employees.Queries.GetByEtablissement
{
    public record GetEmployeesByEtablissementQuery(Guid EtablissementId) : IRequest<List<EmployeeDto>>;
}