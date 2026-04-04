using MediatR;
using Application.Features.Employees.DTOs;
using System;
using System.Collections.Generic;

namespace Application.Features.Employees.Queries.GetDisponibilite
{
    public record GetAvailableEmployeesQuery(Guid EtablissementId, DateTime DateHeure) : IRequest<List<EmployeeDto>>;
}