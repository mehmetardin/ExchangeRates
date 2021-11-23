using ExchangeRates.Domain;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.Interfaces
{
    public interface ICurrencyRepository
    {
        public Task<CurrencyRateApiResponse> GetAvailableExchangeRatesByGivenCurrencyIdAsync(string id);
    }
}
