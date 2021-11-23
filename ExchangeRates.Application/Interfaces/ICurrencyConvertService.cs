﻿using ExchangeRates.Application.Core;
using ExchangeRates.Application.Dto;
using System.Threading.Tasks;

namespace ExchangeRates.Application.Interfaces
{
    public interface ICurrencyConvertService
    {
        public Task<Result<CurrencyConvertResponse>> Convert(CurrencyConvertRequest currencyConvertRequest);
    }
}
