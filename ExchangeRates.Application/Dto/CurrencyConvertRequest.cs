namespace ExchangeRates.Application.Dto
{
    public class CurrencyConvertRequest
    {
        public decimal Amount { get; set; }
        public string SourceCurrencyId { get; set; }
        public string TargetCurrencyId { get; set; }
    }
}
