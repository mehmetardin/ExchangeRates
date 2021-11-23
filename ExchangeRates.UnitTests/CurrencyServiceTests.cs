using ExchangeRates.Application.Dto;
using ExchangeRates.Application.Interfaces;
using ExchangeRates.Application.Services;
using ExchangeRates.Application.Validations;
using ExchangeRates.Domain;
using ExchangeRates.Persistence.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.UnitTests
{
    public class CurrencyServiceTests
    {
        private CurrencyConvertRequestValidator _validator;
        private ICurrencyConvertService _currencyConvertService;
        private Mock<ICurrencyRepository> _currencyRepository;


        [SetUp]
        public void Setup()
        {
            var currencyRateList = new Rate { EUR = 1, GBP = 2, USD = 3 };
            var currencyRate = new CurrencyRateApiResponse { Base = "GBP", Date = DateTime.Now, time_last_updated = 1, Rates = currencyRateList };

            _currencyRepository = new Mock<ICurrencyRepository>();
            _currencyRepository.Setup(m => m.GetAvailableExchangeRatesByGivenCurrencyIdAsync("GBP")).Returns(Task.FromResult(currencyRate)).Verifiable();

            _validator = new CurrencyConvertRequestValidator();

            _currencyConvertService = new CurrencyConvertService(_currencyRepository.Object, _validator);
        }

        [Test]
        public void Convert_WhenTargetCurrencyRateNotAvailable_ExpectArgumentOutOfRangeException()
        {
            var request = new CurrencyConvertRequest
            {
                Amount = 0,
                SourceCurrencyId = "ABC",
                TargetCurrencyId = "USD"
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Convert_WhenRequestedAmountLessesOrEqualToZero_ExpectArgumentOutOfRangeException()
        {
            var request = new CurrencyConvertRequest
            {
                Amount = 0,
                SourceCurrencyId = "GBP",
                TargetCurrencyId = "USD"
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Convert_WhenTargetCurrencyNotAvailable_ExpectArgumentOutOfRangeException()
        {
            var request = new CurrencyConvertRequest
            {
                Amount = 0,
                SourceCurrencyId = "GBP",
                TargetCurrencyId = "XXX"
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Convert_WhenSourceCurrencyNotAvailable_ExpectKeyNotFoundException()
        {
            var request = new CurrencyConvertRequest
            {
                Amount = 1,
                SourceCurrencyId = "XXX",
                TargetCurrencyId = "USD"
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<KeyNotFoundException>());
        }


        [Test]
        public void Convert_WhenSourceAndTargetCurrencyAreSame_ExpectArgumentOutOfRangeException()
        {
            var request = new CurrencyConvertRequest
            {
                Amount = 1,
                SourceCurrencyId = "GBP",
                TargetCurrencyId = "GBP"
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public async Task Convert_WhenCall_ConvertedAmountShouldSameWithExpectedConvertedAmount()
        {
            var request = new CurrencyConvertRequest
            {
                Amount = 1,
                SourceCurrencyId = "GBP",
                TargetCurrencyId = "USD"
            };

            var convertionResult = await _currencyConvertService.Convert(request);

            var expectedConvertedAmount = 3;

            Assert.That(convertionResult.Value.ConvertedAmount.Equals(expectedConvertedAmount));
        }
    }
}