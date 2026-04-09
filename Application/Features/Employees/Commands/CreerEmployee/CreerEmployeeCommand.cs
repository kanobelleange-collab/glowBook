using MediatR;
using Application.Features.Employees.DTOs;

namespace Application.Features.Employees.Commands.CreerEmployee
{
    public class CreerEmployeeCommand : IRequest<EmployeeDto>
    {
        public Guid EtablissementId { get; set; }
        public string? Nom { get; set; }
        public  required string Prenom { get; set; }
        public  required string Specialite { get; set; }
        public string? UrlPhoto { get; set; }
        public int AnneeExperience { get; set; }

     
}


       
    }
