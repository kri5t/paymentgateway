using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Model;
using PaymentGatewayCore.Command;
using PaymentGatewayCore.Query;

namespace PaymentGatewayAPI.Controller
{
    [Route("api/v1/payments")]
    public class PaymentController : BaseController
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Endpoint for carrying out credit card payments
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/payments
        ///     {
        ///        "cardNumber": 1234123412341234,
        ///        "expiryMonth": 12,
        ///        "expiryYear": 2020,
        ///        "amount": 60,
        ///        "currency": DKK,
        ///        "cvv": 123
        ///     }
        /// </remarks>
        /// <param name="model"> The model that has information about the transaction </param>
        /// <returns> OK - If the transaction was a success </returns>
        [HttpPost("")]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequest model) => 
            OkResult(await Mediator.Send(new CreatePaymentCommand(model.CardNumber, model.ExpiryMonth,
                model.ExpiryYear, model.Amount, model.Currency, model.Cvv)));

        /// <summary>
        /// Get a list of payments
        /// </summary>
        /// <returns> List of payments </returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(GetListOfPaymentsResponse), 200)]
        public async Task<IActionResult> GetListOfPayments() =>
            ProjectToResult<GetListOfPaymentsResponse>(await Mediator.Send(new GetPaymentsQuery()));
        
        /// <summary>
        /// Get a specific payment
        /// </summary>
        /// <returns> Detailed payment </returns>
        [HttpGet("{paymentUid}")]
        [ProducesResponseType(typeof(DetailedPaymentResponse), 200)]
        public async Task<IActionResult> GetPayment([FromRoute] Guid paymentUid) =>
            ProjectToResult<DetailedPaymentResponse>(await Mediator.Send(new GetPaymentQuery(paymentUid)));
    }
}