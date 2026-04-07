using MediatR;
using Application.Features.Prestations.DTOs;
using Domain.Entities;


namespace Application.Features.Prestations.Queries.GetById
{
    

public record GetPrestationByIdQuery(Guid Id) : IRequest<Prestation?>;
}