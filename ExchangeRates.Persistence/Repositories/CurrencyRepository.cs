using ExchangeRates.Domain.Entities;
using ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Interfaces;
using ExchangeRates.Persistence.Interfaces;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IExternalCurrencyService _currencyService;

        public CurrencyRepository(IExternalCurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<CurrencyRate> GetTargetCurrencyExchangeRateByGivenSourceCurrencyId(string sourceCurrencyId, string targetCurrencyId)
        {
            return await _currencyService.GetCurrencyAndAvailableRatesAsync(sourceCurrencyId, targetCurrencyId);
        }


    }
}
