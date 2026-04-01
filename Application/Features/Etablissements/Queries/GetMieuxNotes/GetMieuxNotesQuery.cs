using Application.Features.Etablissements.DTOs;
using MediatR;

namespace Application.Features.Etablissements.Queries.GetMieuxNotes
{
    public class GetMieuxNotesQuery : IRequest<List<EtablissementDto>>
    {
        public int Top { get; set; } = 10;
    }
}