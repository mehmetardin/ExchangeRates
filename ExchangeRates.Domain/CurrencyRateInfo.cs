using System;

namespace ExchangeRates.Domain
{
    public class CurrencyRateInfo
    {
        public string Id { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
