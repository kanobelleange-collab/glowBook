using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Application.Features.Payements.Interfaces;
using Domain.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public class CinetPayService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _siteId;
        private readonly string _notifyUrl;

        public CinetPayService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["CinetPay:ApiKey"] ?? throw new ArgumentNullException("CinetPay:ApiKey is missing");
            _siteId = config["CinetPay:SiteId"] ?? throw new ArgumentNullException("CinetPay:SiteId is missing");
            _notifyUrl = config["CinetPay:NotifyUrl"] ?? throw new ArgumentNullException("CinetPay:NotifyUrl is missing");
        }

        public async Task<string> CreerSessionPaiementAsync(
            Guid rendezVousId,
            decimal montant,
            string methodePaiement,
            string urlRetourSucces,
            string urlRetourEchec)
        {
            // 1. Détermination du canal de paiement (Mobile Money par défaut pour le Cameroun)
            var selectedChannels = methodePaiement?.ToUpper() switch
            {
                "ORANGEMONEY" => "MOBILE_MONEY",
                "MTNMOMO" => "MOBILE_MONEY",
                "CARD" => "CREDIT_CARD",
                _ => "ALL" 
            };

            // 2. Construction du corps de la requête (Body)
            // Note : 'amount' doit être un entier pour l'API CinetPay en XAF
            var body = new
            {
                apikey = _apiKey,
                site_id = _siteId,
                transaction_id = rendezVousId.ToString(),
                amount = (int)montant, 
                currency = "XAF",
                description = "Réservation GlowBook",
                notify_url = _notifyUrl,
                return_url = urlRetourSucces,
                cancel_url = urlRetourEchec,
                channels = selectedChannels,
                metadata = "" 
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "https://api-checkout.cinetpay.com/v2/payment", body);

                // Si l'API renvoie une erreur (ex: 400), on récupère le message détaillé
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    throw new Exception($"CinetPay API Error ({(int)response.StatusCode}): {errorDetail}");
                }

                var result = await response.Content.ReadFromJsonAsync<CinetPayResponse>();

                return result?.Data?.PaymentUrl ?? throw new InvalidOperationException("La réponse CinetPay ne contient pas d'URL de paiement.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'initialisation du paiement : {ex.Message}", ex);
            }
        }

        public async Task<bool> VerifierPaiementAsync(string transactionId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "https://api-checkout.cinetpay.com/v2/payment/check",
                    new 
                    { 
                        apikey = _apiKey, 
                        site_id = _siteId, 
                        transaction_id = transactionId 
                    });

                if (!response.IsSuccessStatusCode) return false;

                var result = await response.Content.ReadFromJsonAsync<CinetPayResponse>();
                
                // CinetPay retourne 'ACCEPTED' quand le paiement est validé
                return result?.Data?.Status == "ACCEPTED";
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RembourserAsync(string transactionId, decimal montant)
        {
            // Implémentation future si nécessaire
            return await Task.FromResult(true);
        }
    }
}