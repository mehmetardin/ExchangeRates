namespace ExchangeRates.Application.Dto
{
    public class CurrencyConvertResponse
    {
        public CurrencyConvertResponse(decimal amount, string sourceCurrencyId, string targetCurrencyId)
        {
            Amount = amount;
            SourceCurrencyId = sourceCurrencyId;
            TargetCurrencyId = targetCurrencyId;
        }

        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal ConvertedAmount { get; set; }
        public string SourceCurrencyId { get; set; }
        public string TargetCurrencyId { get; set; }
        
    }
}
