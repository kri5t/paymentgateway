using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreatePaymentRequest() =>
            OkResult(await Mediator.Send(new CreatePaymentCommand())); 
    }
}