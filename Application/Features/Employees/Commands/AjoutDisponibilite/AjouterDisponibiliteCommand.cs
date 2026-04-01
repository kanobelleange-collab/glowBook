using MediatR;
using Application.Features.Employees.DTOs;
using Domain.Entities;

namespace Application.Features.Employees.Commands.AjoutDisponibilite
{
    public class AjouterDisponibiliteCommand : IRequest<EmployeeDto>
    {
        public Guid EmployeeId { get; set; }
        public  required string Jour { get; set; }
        // ex : "Monday", "Tuesday"...
        public required  string HeureDebut { get; set; }
        // ex : "09:00"
        public required string HeureFin { get; set; }
        // ex : "18:00"
    }
}