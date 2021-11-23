using ExchangeRates.Domain;
using ExchangeRates.Persistence.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
       

        //We can use HttpClientFactory and use it via dependency injection later
        public async Task<CurrencyRateApiResponse> GetAvailableExchangeRatesByGivenCurrencyIdAsync(string id)
        {
            var url = $"https://trainlinerecruitment.github.io/exchangerates/api/latest/{id.ToUpper()}.json";
            HttpClient client = new();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CurrencyRateApiResponse>(result);
            }

            return null;
        }

       

        
    }
}
