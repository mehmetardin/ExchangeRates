using ExchangeRates.Domain;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Persistence.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public async Task<CurrencyRate> GetTargetCurrencyExchangeRateByGivenSourceCurrencyId(string sourceCurrencyId, string targetCurrencyId)
        {
            return await GetCurrencyAndAvailableRatesAsync(sourceCurrencyId, targetCurrencyId);
        }

        //We can use HttpClientFactory and use it via dependency injection later
        private static async Task<CurrencyRate> GetCurrencyAndAvailableRatesAsync(string sourceCurrencyId, string targetCurrencyId)
        {
            var url = $"https://trainlinerecruitment.github.io/exchangerates/api/latest/{sourceCurrencyId.ToUpper()}.json";
            HttpClient client = new();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;

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
            var properties = ratesType.GetProperties();

            var result = new CurrencyRate { SourceCurrencyId = rates.Base, TargetCurrency = null };
            foreach (var property in properties)
            {
                if (property.Name.ToString().ToUpper() == targetCurrencyId.ToUpper())
                {
                    var value = ratesType.GetProperty(property.Name).GetValue(rates.Rates, null);
                    var currencyRate = new CurrencyRateInfo { Id = targetCurrencyId, Date = rates.Date, Rate = Convert.ToDecimal(value) };
                    result.TargetCurrency = currencyRate;
                }
            }

            return result;
        }
    }
}
