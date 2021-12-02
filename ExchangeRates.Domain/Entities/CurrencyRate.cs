namespace ExchangeRates.Domain.Entities
{
    public class CurrencyRate
    {
        public string SourceCurrencyId { get; set; }
        public CurrencyRateInfo TargetCurrency { get; set; }
    }
}
