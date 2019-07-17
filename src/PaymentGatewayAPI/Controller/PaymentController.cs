using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Model;
using PaymentGatewayCore.Command;

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
        /// <param name="model"> The model that has information about the transaction </param>
        /// <returns> OK - If the transaction was a success </returns>
        [HttpPost("")]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequest model)
        {
            return OkResult(await Mediator.Send(new CreatePaymentCommand(model.CardNumber, model.ExpiryMonth,
                model.ExpiryYear, model.Amount, model.Currency, model.Cvv)));
        }
    }
}