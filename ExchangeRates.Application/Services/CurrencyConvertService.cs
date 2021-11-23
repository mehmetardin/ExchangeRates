using ExchangeRates.Application.Core;
using ExchangeRates.Application.Dto;
using ExchangeRates.Application.Interfaces;
using ExchangeRates.Domain;
using ExchangeRates.Persistence.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExchangeRates.Application.Services
{
    public class CurrencyConvertService : ICurrencyConvertService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IValidator<CurrencyConvertRequest> _validator;
        private List<CurrencyRate> _availableCurrencyRates = new();

        public CurrencyConvertService(ICurrencyRepository currencyRepository, IValidator<CurrencyConvertRequest> validator)
        {
            _currencyRepository = currencyRepository;
            _validator = validator;
        }

        public async Task<Result<CurrencyConvertResponse>> Convert(CurrencyConvertRequest currencyConvertRequest)
        {
            await ValidateArgumentsAsync(currencyConvertRequest);

            var availableExchangeRates = await _currencyRepository.GetAvailableExchangeRatesByGivenCurrencyIdAsync(currencyConvertRequest.SourceCurrencyId);

            if (availableExchangeRates == null)
                throw new KeyNotFoundException($"{currencyConvertRequest.SourceCurrencyId} is not a supported currency");

            SetAvailableCurrencyRates(availableExchangeRates);

            var targerCurrencyRate = GetCurrencyRateByCurrencyId(currencyConvertRequest.TargetCurrencyId.ToUpper()).Rate;

            var result = new CurrencyConvertResponse(currencyConvertRequest.Amount, currencyConvertRequest.SourceCurrencyId, currencyConvertRequest.TargetCurrencyId);

            result.ConvertedAmount = targerCurrencyRate * result.Amount;
            result.Rate = targerCurrencyRate;

            return Result<CurrencyConvertResponse>.Success(result);
        }

        private async Task ValidateArgumentsAsync(CurrencyConvertRequest currencyConvertRequest)
        {
            var validationResult = await _validator.ValidateAsync(currencyConvertRequest);

            if (!validationResult.IsValid)
            {
                var errors = string.Empty;

                foreach (var failure in validationResult.Errors)
                    errors += "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage;

                throw new ArgumentOutOfRangeException(errors);
            }
        }

        private void SetAvailableCurrencyRates(CurrencyRateApiResponse rates)
        {
            Type ratesType = rates.Rates.GetType();
            PropertyInfo[] properties = ratesType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var rate = new CurrencyRate();
                rate.CurrencyId = property.Name.ToString().ToUpper();

                var value = rates.Rates.GetType().GetProperty(property.Name.ToString()).GetValue(rates.Rates, null);
                rate.Rate = System.Convert.ToDecimal(value);

                _availableCurrencyRates.Add(rate);
            }
        }

        private CurrencyRate GetCurrencyRateByCurrencyId(string targetCurrencyId)
        {
            var currencyRate = _availableCurrencyRates.FirstOrDefault(c => c.CurrencyId == targetCurrencyId);

            if (currencyRate == null)
                throw new ArgumentOutOfRangeException($"{targetCurrencyId} is not available");

            return currencyRate;
        }

    }
}
