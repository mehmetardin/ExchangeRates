using ExchangeRates.Application.Dto;
using ExchangeRates.Application.Interfaces;
using ExchangeRates.Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.Application.Services
{
    public class CurrencyConvertService : ICurrencyConvertService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IValidator<CurrencyConvertRequest> _validator;

        public CurrencyConvertService(ICurrencyRepository currencyRepository, IValidator<CurrencyConvertRequest> validator)
        {
            _currencyRepository = currencyRepository;
            _validator = validator;
        }

        public async Task<CurrencyConvertResponse> Convert(CurrencyConvertRequest currencyConvertRequest)
        {
            await ValidateArgumentsAsync(currencyConvertRequest);

            var exchangeRateInfo = await _currencyRepository.GetTargetCurrencyExchangeRateByGivenSourceCurrencyId(currencyConvertRequest.SourceCurrencyId, currencyConvertRequest.TargetCurrencyId);

            if (exchangeRateInfo == null)
                throw new KeyNotFoundException($"{currencyConvertRequest.SourceCurrencyId} is not a supported currency");

            if(exchangeRateInfo.TargetCurrency == null)
                throw new KeyNotFoundException($"{currencyConvertRequest.TargetCurrencyId} is not a supported currency");

            return new CurrencyConvertResponse
            {
                Amount              = currencyConvertRequest.Amount,
                SourceCurrencyId    = currencyConvertRequest.SourceCurrencyId,
                TargetCurrencyId    = currencyConvertRequest.TargetCurrencyId,
                ConvertedAmount     = exchangeRateInfo.TargetCurrency.Rate * currencyConvertRequest.Amount,
                Rate                = exchangeRateInfo.TargetCurrency.Rate
            };
        }

        private async Task ValidateArgumentsAsync(CurrencyConvertRequest currencyConvertRequest)
        {
            var validationResult = await _validator.ValidateAsync(currencyConvertRequest);

            if (!validationResult.IsValid)
                throw new ArgumentOutOfRangeException(string.Join(", ", validationResult.Errors));
        }

    }
}
