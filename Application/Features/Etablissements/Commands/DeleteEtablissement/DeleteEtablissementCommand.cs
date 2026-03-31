using System;
using MediatR;
namespace Application.Features.Etablissements.Commands.DeleteEtablissement
{
    public class DeleteEtablissementCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}