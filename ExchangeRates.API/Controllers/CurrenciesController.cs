using ExchangeRates.Application.Dto;
using ExchangeRates.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeRates.API.Controllers
{
    public class CurrenciesController : BaseApiController
    {
        private readonly ICurrencyConvertService _currencyConvertService;

        public CurrenciesController(ICurrencyConvertService currencyConvertService)
        {
            _currencyConvertService = currencyConvertService;
        }

        [HttpGet]
        [Route("Convert")]
        public async Task<IActionResult> Convert([FromQuery] CurrencyConvertRequest currencyConvertRequest)
        {
            return HandleResult(await _currencyConvertService.Convert(currencyConvertRequest));
        }
    }
}
