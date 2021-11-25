using System;

namespace ExchangeRates.Domain.Entities
{
    public class CurrencyRates
    {
        public string SourceCurrencyId { get; set; }
        public CurrencyRate CurrencyRate { get; set; }  
    }
}
