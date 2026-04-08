using MediatR;
using Application.Features.Prestations.Interfaces;
using Application.Features.Prestations.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Prestations.Commands.UpdatePrestation
{
    public class UpdatePrestationCommandHandler : IRequestHandler<UpdatePrestationCommand, PrestationDto>
    {
        private readonly IPrestationRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePrestationCommandHandler(IPrestationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PrestationDto> Handle(UpdatePrestationCommand request, CancellationToken cancellationToken)
        {
            var prestation = await _repository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Prestation {request.Id} introuvable");

            prestation.Nom         = request.Nom;
            prestation.Description = request.Description;
            prestation.Prix        = request.Prix;
            // prestation.ServiceId   = request.ServiceId;
            prestation.DureeMinutes=request.DureeMinutes;

            await _repository.UpdateAsync(prestation);

            // ✅ Retourne PrestationDto — pas Prestation
            return _mapper.Map<PrestationDto>(prestation);
        }
    }
}