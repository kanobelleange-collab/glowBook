namespace Application.Features.Aviss.DTOs;
public record AvisDto(
    Guid Id,
    int Note,
    string Commentaire,
    DateTime DateAvis,
    Guid ClientId,
    string? ReponseEtablissement,
    string EtoilesFormatees // Pratique pour l'affichage Flutter/React
);