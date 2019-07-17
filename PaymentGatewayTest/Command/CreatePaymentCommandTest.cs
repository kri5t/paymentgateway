using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Moq;
using PaymentGatewayCore.BankMock;
using PaymentGatewayCore.Command;
using PaymentGatewayCore.Repository;
using PaymentGatewayDatabase.Models;
using Xunit;

namespace PaymentGatewayTest.Command
{
    public class CreatePaymentCommandTest
    {
        public CreatePaymentCommandTest()
        {
            _createdDateUtc = DateTimeOffset.UtcNow;
            _transactionUid = Guid.NewGuid();
            _paymentRepository = new Mock<IPaymentRepository>();

            var systemClock = new Mock<ISystemClock>();
            systemClock.Setup(d => d.UtcNow).Returns(_createdDateUtc);
            _bank = new Mock<IBank>();
            _sut = new CreatePaymentCommandHandler(_paymentRepository.Object, _bank.Object, systemClock.Object);
        }

        private readonly DateTimeOffset _createdDateUtc;
        private readonly Mock<IPaymentRepository> _paymentRepository;
        private readonly CreatePaymentCommandHandler _sut;
        private readonly Guid _transactionUid;
        private readonly Mock<IBank> _bank;
        private const string CardNumber = "1234123412341234";
        private const string Currency = "DKK";
        private const int Amount = 30;
        private const int ExpiryMonth = 10;
        private const int ExpiryYear = 2030;
        private const int Cvv = 123;

        private Expression<Func<IBank, (PaymentStatus, Guid)>> BankExpression =>
            x => x.CreateTransfer(
                It.Is<string>(c => c == CardNumber),
                It.Is<int>(s => s == ExpiryMonth),
                It.Is<int>(s => s == ExpiryYear),
                It.Is<int>(s => s == Amount),
                It.Is<string>(s => s == Currency),
                It.Is<int>(s => s == Cvv)
            );
        
        private void SetupBank(PaymentStatus paymentStatus) =>
            _bank.Setup(BankExpression).Returns((paymentStatus, _transactionUid));
        
        [Fact]
        public async Task Calls_Bank()
        {
            //Setup
            var command = new CreatePaymentCommand(CardNumber, ExpiryMonth, ExpiryYear, Amount, Currency, Cvv);
            SetupBank(PaymentStatus.PaymentSucceeded);

            //Test
            await _sut.Handle(command, CancellationToken.None);

            //Verify
            _bank.Verify(BankExpression, Times.Once);
        }

        [Fact]
        public async Task Calls_CreatePaymentAsync()
        {
            //Setup
            var command = new CreatePaymentCommand(CardNumber, ExpiryMonth, ExpiryYear, Amount, Currency, Cvv);
            var cancellationToken = CancellationToken.None;
            var paymentStatus = PaymentStatus.PaymentSucceeded;
            SetupBank(paymentStatus);

            //Test
            await _sut.Handle(command, cancellationToken);

            //Verify
            _paymentRepository.Verify(x => x.CreatePaymentAsync(
                It.Is<CancellationToken>(c => c == cancellationToken),
                It.Is<string>(s => s == CardNumber),
                It.Is<int>(i => i == Amount),
                It.Is<string>(i => i == Currency),
                It.Is<PaymentStatus>(i => i == paymentStatus),
                It.Is<Guid>(i => i == _transactionUid),
                It.Is<DateTimeOffset>(d => d == _createdDateUtc)), Times.Once);
        }

        [Fact]
        public async Task Calls_SaveAsync()
        {
            //Setup
            var command = new CreatePaymentCommand(CardNumber, ExpiryMonth, ExpiryYear, Amount, Currency, Cvv);
            var cancellationToken = CancellationToken.None;

            //Test
            await _sut.Handle(command, cancellationToken);

            //Verify
            _paymentRepository.Verify(x => x.SaveAsync(It.Is<CancellationToken>(c => c == cancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task ReturnsError_WhenExternalProviderFails()
        {
            //Setup
            SetupBank(PaymentStatus.PaymentDeclined);
            var command = new CreatePaymentCommand(CardNumber, ExpiryMonth, ExpiryYear, Amount, Currency, Cvv);

            //Test
            var result = await _sut.Handle(command, CancellationToken.None);

            //Verify
            Assert.False(result.IsSuccess);
            Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
        }

        [Fact]
        public async Task ReturnsSuccess_WhenCalledWithCorrectParameters()
        {
            //Setup
            SetupBank(PaymentStatus.PaymentSucceeded);
            var command = new CreatePaymentCommand(CardNumber, ExpiryMonth, ExpiryYear, Amount, Currency, Cvv);

            //Test
            var result = await _sut.Handle(command, CancellationToken.None);

            //Verify
            Assert.True(result.IsSuccess);
        }
    }
}