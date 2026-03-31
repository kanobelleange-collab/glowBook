using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;
namespace Application.Features.Rendevou.GetRendezVousByPraticien;
public record GetRendezVousByPraticienQuery(Guid PraticienId) : IRequest<List<RendezVousDto>>;
