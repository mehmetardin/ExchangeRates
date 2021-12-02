using System;
using System.Collections.Generic;

namespace ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Dto
{
    internal class CurrencyRateApiResponse
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public int time_last_updated { get; set; }
        public Dictionary<string,decimal> Rates { get; set; }
    }
}
