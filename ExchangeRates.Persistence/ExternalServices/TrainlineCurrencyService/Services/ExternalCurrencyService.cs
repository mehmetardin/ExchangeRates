using ExchangeRates.Domain;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Dto;
using ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService
{
    public class ExternalCurrencyService : IExternalCurrencyService
    {
        private readonly HttpClient _client;

        public ExternalCurrencyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<CurrencyRate> GetCurrencyAndAvailableRatesAsync(string sourceCurrencyId, string targetCurrencyId)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync($"latest/{sourceCurrencyId.ToUpper()}.json");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<CurrencyRateApiResponse>(result);
                return GetTargetCurrencyRate(apiResult, targetCurrencyId);
            }

            return null;
        }

        private static CurrencyRate GetTargetCurrencyRate(CurrencyRateApiResponse rates, string targetCurrencyId)
        {
            var ratesType = rates.Rates.GetType();

            var result = new CurrencyRate { SourceCurrencyId = rates.Base };

            foreach (var property in ratesType.GetProperties())
            {
                if (property.Name.ToString().ToUpper() == targetCurrencyId.ToUpper())
                {
                    var targetCurrencyRate = ratesType.GetProperty(property.Name).GetValue(rates.Rates, null);
                    result.TargetCurrency = new CurrencyRateInfo { Id = targetCurrencyId, Date = rates.Date, Rate = Convert.ToDecimal(targetCurrencyRate) };
                }
            }

            return result;
        }
    }
}
