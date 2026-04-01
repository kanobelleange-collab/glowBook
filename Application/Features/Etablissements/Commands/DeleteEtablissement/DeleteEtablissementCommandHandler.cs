// Application/Features/Etablissements/Commands/Delete/DeleteEtablissementCommand.cs
using MediatR;
using Application.Features.Etablissements.Interfaces;
using AutoMapper;


namespace Application.Features.Etablissements.Commands.DeleteEtablissement
{
   
    public class DeleteEtablissementCommandHandler
        : IRequestHandler<DeleteEtablissementCommand, bool>
    {
        private readonly IEtablissementRepository _repository;

        public DeleteEtablissementCommandHandler(
            IEtablissementRepository repository)
            => _repository = repository;

        public async Task<bool> Handle(
            DeleteEtablissementCommand request,
            CancellationToken cancellationToken)
        {
            // ✅ Vérifier que l'établissement existe
            var etablissement = await _repository.GetByIdAsync(request.Id)
                ?? throw new Exception("Établissement introuvable.");

            // ✅ Soft delete via repository
            await _repository.DeleteAsync(request.Id);
            return true;
        }
    }
}