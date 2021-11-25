using ExchangeRates.Domain.Entities;
using System.Threading.Tasks;

namespace ExchangeRates.Persistence.Interfaces
{
    public interface ICurrencyRepository
    {
        public Task<CurrencyRates> GetTargetCurrencyExchangeRateByGivenSourceCurrencyId(string sourceCurrencyId, string targetCurrencyId);

    }
}
