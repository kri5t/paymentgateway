using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayCore.Model;

namespace PaymentGatewayAPI.Controller
{
    public class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected IMediator Mediator { get; }

        protected IActionResult ProjectToResult<T>(BaseResponseModel model)
        {
            return !model.IsSuccess ? 
                ConvertToHttpResponse(model.ErrorCode, model.ErrorMessage) : 
                Ok(Mapper.Map<T>(model));
        }

        protected IActionResult OkResult(BaseResponseModel model)
        {
            return !model.IsSuccess ? 
                ConvertToHttpResponse(model.ErrorCode, model.ErrorMessage) : 
                Ok();
        }

        private IActionResult ConvertToHttpResponse(
            ErrorCode errorCode,
            string message)
        {
            switch (errorCode)
            {
                case ErrorCode.NotFound:
                    return NotFound(message);
                case ErrorCode.UnexpectedError:
                    return BadRequest(message);
                case ErrorCode.NoAccess:
                    return StatusCode(403, message);
                default:
                    throw new Exception($"Invalid ErrorCode '{(object) errorCode}', cannot return error");
            }
        }
    }
}