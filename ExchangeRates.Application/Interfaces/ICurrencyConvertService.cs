using ExchangeRates.Application.Dto;
using System.Threading.Tasks;

namespace ExchangeRates.Application.Interfaces
{
    public interface ICurrencyConvertService
    {
        public Task<CurrencyConvertResponse> Convert(CurrencyConvertRequest currencyConvertRequest);
    }
}
