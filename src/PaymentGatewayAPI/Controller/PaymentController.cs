using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Model;
using PaymentGatewayCore.Command;

namespace PaymentGatewayAPI.Controller
{
    [Route("api/v1/payment")]
    public class PaymentController : BaseController
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequest model) =>
            OkResult(await Mediator.Send(
                new CreatePaymentCommand(model.CardNumber, model.ExpiryMonth, model.ExpiryYear, 
                    model.Amount, model.Currency, model.Cvv))); 
    }
}