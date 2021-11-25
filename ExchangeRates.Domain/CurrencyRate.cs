using System;

namespace ExchangeRates.Domain
{
    public class CurrencyRate
    {
        public string CurrencyId { get; set; }
        public decimal Rate { get; set; }
        public DateTime CurrenyDate { get; set; }
    }
}
