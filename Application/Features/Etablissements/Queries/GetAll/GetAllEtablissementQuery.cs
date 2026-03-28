using System;

using MediatR;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Queries.GetAll
{
    public class GetAllEtablissementQuery : IRequest<List<EtablissementDto>>
    {
    }
}