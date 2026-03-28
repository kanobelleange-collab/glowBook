using AutoMapper;
using MediatR;
using Application.Features.ServiceEsthetiques.DTOs;
using Application.Features.ServiceEsthtiques.Interfaces;
using Domain.Entities;
using  Application.Features.ServiceEsthetiques.Commands.CreerServiceEsthetique;

namespace Application.Features.ServiceEsthetiques.Commands
{
    public class CreerServiceEsthetiqueCommandHandler
        : IRequestHandler<CreerServiceEsthetiqueCommand, ServiceEsthetiqueDto>
    {
        private readonly IServiceEsthetiqueRepository _serviceRepository;
        private readonly IMapper _mapper;

        public CreerServiceEsthetiqueCommandHandler(
            IServiceEsthetiqueRepository serviceRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper            = mapper;
        }

        public async Task<ServiceEsthetiqueDto> Handle(
            CreerServiceEsthetiqueCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Créer l'entité
            var service = new ServiceEsthetique(
                command.Nom,
                command.Description,
                command.Prix,
                command.DureeEnMinutes,
                command.Categorie,
                command.EtablissementId
            );

            // 2. Sauvegarder en base
            await _serviceRepository.AjouterAsync(service);

            // 3. Mapper l'entité vers le DTO et retourner
            return _mapper.Map<ServiceEsthetiqueDto>(service);
        }
    }
}