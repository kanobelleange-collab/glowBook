using MediatR;
using Application.Features.Employees.DTOs;

namespace Application.Features.Employees.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommand(
        Guid Id,
        string Nom,
        string Prenom,
        string Specialite,
        
        int AnneesExperience,
        string? UrlPhoto = null)
    
     : IRequest<EmployeeDto>;
}