using MediatR;
using Domain.Enum;
using Domain.Entities;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Commands.AddServices.SpaBeaute
{
    public class AddSpaBeauteCommand : IRequest<Guid>
    {
        public Guid EtablissementId { get; set; }
        public ServiceSpa Service { get; set; }
        public List<string> SoinsCorps { get; set; } = new();
        public List<string> Rituels { get; set; } = new();
        public int NombresCabinesPrivees { get; set; } = 0;
        public List<PrestationDto> Prestations { get; set; } = new();
    }
}