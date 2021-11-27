using ExchangeRates.Domain.Entities;
using System.Threading.Tasks;

namespace ExchangeRates.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        public Task<CurrencyRate> GetTargetCurrencyExchangeRateByGivenSourceCurrencyId(string sourceCurrencyId, string targetCurrencyId);

    }
}
