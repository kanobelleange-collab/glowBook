namespace Application.Features.Admin.Dtos
{
    public record AdminStatsDto(
        int TotalClients,
        int TotalSalons,
        int TotalRDV
    );
}