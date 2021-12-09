using ExchangeRates.Application.Dto;
using ExchangeRates.Application.Interfaces;
using ExchangeRates.Application.Services;
using ExchangeRates.Application.Validations;
using ExchangeRates.Domain;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.UnitTests
{
    public class CurrencyConvertServiceTests
    {
        private CurrencyConvertRequestValidator _validator;
        private ICurrencyConvertService _currencyConvertService;
        private Mock<ICurrencyRepository> _currencyRepository;

        [SetUp]
        public void Setup()
        {
            var currencyRate = new CurrencyRateInfo { Id = "USD", Rate = 3, Date = DateTime.Now };

            var currencyWithAvailableCurrencyRates = new CurrencyRate { SourceCurrencyId = "GBP", TargetCurrency = currencyRate };

            _currencyRepository = new Mock<ICurrencyRepository>();
            _currencyRepository.Setup(m => m.GetTargetCurrencyExchangeRateByGivenSourceCurrencyId("GBP", "USD")).Returns(Task.FromResult(currencyWithAvailableCurrencyRates)).Verifiable();

            _validator = new CurrencyConvertRequestValidator();

            _currencyConvertService = new CurrencyConvertService(_currencyRepository.Object, _validator);
        }

       
        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void Convert_WhenRequestedAmountLessesOrEqualToZero_ExpectArgumentOutOfRangeException(int amount)
        {
            var request = new CurrencyConvertRequest
            {
                Amount = amount,
                SourceCurrencyId = "GBP",
                TargetCurrencyId = "USD"
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(1, "GBP", "XXX")]
        public void Convert_WhenTargetCurrencyNotAvailable_ExpectArgumentOutOfRangeException(int amount, string sourceCurrencyId, string targetCurrencyId)
        {
            var request = new CurrencyConvertRequest
            {
                Amount = amount,
                SourceCurrencyId = sourceCurrencyId,
                TargetCurrencyId = targetCurrencyId
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<KeyNotFoundException>());
        }

        [Test]
        [TestCase(1, "XXX", "USD")]
        public void Convert_WhenSourceCurrencyNotAvailable_ExpectKeyNotFoundException(int amount, string sourceCurrencyId, string targetCurrencyId)
        {
            var request = new CurrencyConvertRequest
            {
                Amount = amount,
                SourceCurrencyId = sourceCurrencyId,
                TargetCurrencyId = targetCurrencyId
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<KeyNotFoundException>());
        }


        [Test]
        [TestCase(1, "GBP", "GBP")]
        public void Convert_WhenSourceAndTargetCurrencyAreSame_ExpectArgumentOutOfRangeException(int amount, string sourceCurrencyId, string targetCurrencyId)
        {
            var request = new CurrencyConvertRequest
            {
                Amount = amount,
                SourceCurrencyId = sourceCurrencyId,
                TargetCurrencyId = targetCurrencyId
            };

            Assert.That(() => _currencyConvertService.Convert(request), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(1, 3)]
        [TestCase(2, 6)]
        [TestCase(3, 9)]
        public async Task Convert_WhenCall_ConvertedAmountShouldSameWithExpectedConvertedAmount(int amount, int expected)
        {
            var request = new CurrencyConvertRequest
            {
                Amount = amount,
                SourceCurrencyId = "GBP",
                TargetCurrencyId = "USD"
            };

            var convertionResult = await _currencyConvertService.Convert(request);

            Assert.That(convertionResult.ConvertedAmount.Equals(expected));
        }
    }
}