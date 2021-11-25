using System;

namespace ExchangeRates.Domain.Entities
{
    public class CurrencyRates
    {
        public string SourceCurrencyId { get; set; }
        public string TargetCurrencyId  { get; set; }
        public DateTime CurrenyDate { get; set; }
        public decimal Rate { get; set; }
    }
}
