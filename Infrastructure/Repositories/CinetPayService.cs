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

        public CinetPayService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["CinetPay:ApiKey"] ?? throw new ArgumentNullException(nameof(config));
            _siteId = config["CinetPay:SiteId"] ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<string> CreerSessionPaiementAsync(
            Guid rendezVousId,
            decimal montant,
            string methodePaiement,
            string urlRetourSucces,
            string urlRetourEchec)
        {
            var body = new
            {
                apikey = _apiKey,
                site_id = _siteId,
                transaction_id = rendezVousId.ToString(),
                amount = montant,
                currency = "XAF",
                description = "Réservation GlowBook",
                return_url = urlRetourSucces,
                cancel_url = urlRetourEchec,
                channels = methodePaiement == "OrangeMoney" ? "ORANGE_MONEY" : "MTN_MOMO"
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "https://api-checkout.cinetpay.com/v2/payment", body);

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<CinetPayResponse>();

                return result?.Data?.PaymentUrl ?? throw new InvalidOperationException("PaymentUrl not found");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"CinetPay API error: {ex.Message}", ex);
            }
        }

        public async Task<bool> VerifierPaiementAsync(string transactionId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "https://api-checkout.cinetpay.com/v2/payment/check",
                    new { apikey = _apiKey, site_id = _siteId, transaction_id = transactionId });

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<CinetPayResponse>();

                return result?.Data?.Status == "ACCEPTED";
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"CinetPay verification error: {ex.Message}", ex);
            }
        }

        public async Task<bool> RembourserAsync(string transactionId, decimal montant)
        {
            return await Task.FromResult(true);
        }
    }
}