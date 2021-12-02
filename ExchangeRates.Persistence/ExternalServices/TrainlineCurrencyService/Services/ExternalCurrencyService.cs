using ExchangeRates.Domain;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Dto;
using ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService
{
    public class ExternalCurrencyService : IExternalCurrencyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalCurrencyService(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

        public async Task<CurrencyRate> GetCurrencyAndAvailableRatesAsync(string sourceCurrencyId, string targetCurrencyId)
        {
            var httpClient = _httpClientFactory.CreateClient("TrainLine");

            var response = await httpClient.GetAsync($"latest/{sourceCurrencyId.ToUpper()}.json");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<CurrencyRateApiResponse>(result);
                return ConvertApiResponseToCurrencyRate(apiResult, targetCurrencyId);
            }

            return null;
        }

        private static CurrencyRate ConvertApiResponseToCurrencyRate(CurrencyRateApiResponse apiResult, string targetCurrency)
        {
            return new CurrencyRate
            {
                SourceCurrencyId = apiResult.Base,
                TargetCurrency = new CurrencyRateInfo
                {
                    Date = apiResult.Date,
                    Rate = apiResult.Rates[targetCurrency.ToUpper()],
                    Id = targetCurrency
                }
            };
        }
    }
}
