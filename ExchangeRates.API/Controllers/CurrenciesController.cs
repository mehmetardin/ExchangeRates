using ExchangeRates.Application.Dto;
using ExchangeRates.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeRates.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrenciesController : ControllerBase
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
            return Ok(await _currencyConvertService.Convert(currencyConvertRequest));
        }
    }
}
