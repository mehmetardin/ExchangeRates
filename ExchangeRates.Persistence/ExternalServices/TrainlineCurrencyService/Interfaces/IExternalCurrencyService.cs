using ExchangeRates.Domain.Entities;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Interfaces
{
    public interface IExternalCurrencyService
    {
        public Task<CurrencyRate> GetCurrencyAndAvailableRatesAsync(string sourceCurrencyId, string targetCurrencyId);
    }
}
