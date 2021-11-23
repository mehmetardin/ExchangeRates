using ExchangeRates.Application.Dto;
using FluentValidation;

namespace ExchangeRates.Application.Validations
{
    public class CurrencyConvertRequestValidator : AbstractValidator<CurrencyConvertRequest>
    {
        public CurrencyConvertRequestValidator()
        {
            RuleFor(item => item)
                   .Must(item => item.Amount > 0)
                   .WithMessage("Requested amount must be bigger than zero");

            RuleFor(item => item)
                .Must(item => item.SourceCurrencyId != item.TargetCurrencyId)
                .WithMessage("Source and Target Currencies cannot be same!");

            RuleFor(x => x.SourceCurrencyId).NotNull();

            RuleFor(x => x.TargetCurrencyId).NotNull();
        }
    }
}
