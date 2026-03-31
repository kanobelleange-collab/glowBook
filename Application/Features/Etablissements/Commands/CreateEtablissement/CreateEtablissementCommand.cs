using MediatR;
using Application.Features.Etablissements.DTOs;
using Domain.Enum;
 


namespace Application.Features.Etablissements.Commands.CreateEtablissement
{
   // CreateEtablissementBaseCommand.cs
public  record CreateEtablissementCommand : IRequest<EtablissementDto>
{
    public  string Nom { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Quartier { get; set; } = string.Empty;
    public  string Ville { get; set; } = string.Empty;
    public  string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description{get;set;} = string.Empty;
    public List<string>? Photos { get; set; }
    public List<HoraireOuvertureDto>? Horaires { get; set; }
}
}