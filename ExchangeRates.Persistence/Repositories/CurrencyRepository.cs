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
        //We can use HttpClientFactory and use it via dependency injection later
        public async Task<CurrencyRates> GetTargetCurrencyExchangeRateByGivenSourceCurrencyId(string sourceCurrencyId, string targetCurrencyId)
        {
            return await GetCurrencyAndAvailableRatesAsync(sourceCurrencyId, targetCurrencyId);
        }

        private static async Task<CurrencyRates> GetCurrencyAndAvailableRatesAsync(string sourceCurrencyId, string targetCurrencyId)
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

        private static CurrencyRates GetTargetCurrencyRate(CurrencyRateApiResponse rates, string targetCurrencyId)
        {
            var ratesType = rates.Rates.GetType();
            var properties = ratesType.GetProperties();

            var result = new CurrencyRates { SourceCurrencyId = rates.Base, CurrencyRate = null };
            foreach (var property in properties)
            {
                if (property.Name.ToString().ToUpper() == targetCurrencyId.ToUpper())
                {
                    var value = ratesType.GetProperty(property.Name).GetValue(rates.Rates, null);
                    var currencyRate = new CurrencyRate { CurrencyId = targetCurrencyId, CurrenyDate = rates.Date, Rate = Convert.ToDecimal(value) };
                    result.CurrencyRate = currencyRate;
                }
            }

            return result;
        }
    }
}
