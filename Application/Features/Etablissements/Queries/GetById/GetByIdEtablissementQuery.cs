using System;
using MediatR;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Queries.GetById
{
    public class GetByIdEtablissementQuery : IRequest<EtablissementDto>
    {
        public Guid Id { get; set; }
    }
}