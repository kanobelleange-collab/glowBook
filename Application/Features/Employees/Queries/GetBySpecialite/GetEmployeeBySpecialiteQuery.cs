using MediatR;
using Application.Features.Employees.DTOs;
using System.Collections.Generic;

namespace Application.Features.Employees.Queries.GetBySpecialite
{
    public record GetEmployeeBySpecialiteQuery(string Specialite) : IRequest<List<EmployeeDto>>;
}