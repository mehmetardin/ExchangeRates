using System;

namespace ExchangeRates.Persistence
{
    internal class CurrencyRateApiResponse
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public int time_last_updated { get; set; }
        public Rate Rates { get; set; }
    }
}
