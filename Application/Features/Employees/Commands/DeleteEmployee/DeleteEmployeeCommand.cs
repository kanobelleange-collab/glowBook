using System;


using MediatR;

namespace Application.Features.Employees.Commands.DeleteEmployee
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest;
}