using MediatR;
using Application.Features.Employees.DTOs;
using Domain.Entities;

namespace Application.Features.Employees.Queries.GetById
{
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<Employee?>;
}