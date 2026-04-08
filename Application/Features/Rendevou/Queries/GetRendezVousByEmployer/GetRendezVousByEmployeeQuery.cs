using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;
namespace Application.Features.Rendevou.GetRendezVousByEmployer;
public record GetRendezVousByEmployeeQuery(Guid PraticienId) : IRequest<List<RendezVousDto>>;
