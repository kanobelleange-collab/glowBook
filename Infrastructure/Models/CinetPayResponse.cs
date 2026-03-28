

using Domain.Entities;


namespace Infrastructure.Models
{

    // Réponse globale de l'API CinetPay
    public class CinetPayResponse
    {
        public string? Code { get; set; }
        public  required string Message { get; set; }
        public  required CinetPayData Data { get; set; }
    }

    // Données retournées par CinetPay
    public class CinetPayData
    {
        public  required string PaymentUrl { get; set; }
        public required string Status { get; set; }
        public  required string TransactionId { get; set; }
      }
}
