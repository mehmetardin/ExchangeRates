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
        private const string _baseUrl = "https://trainlinerecruitment.github.io/exchangerates/api";

        public ExternalCurrencyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<CurrencyRate> GetCurrencyAndAvailableRatesAsync(string sourceCurrencyId, string targetCurrencyId)
        {
            var url = $"{_baseUrl}/latest/{sourceCurrencyId.ToUpper()}.json";
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = _client.GetAsync(url).Result;

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
