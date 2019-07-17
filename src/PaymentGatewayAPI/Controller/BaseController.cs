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

        /// <summary>
        /// Used for dispatching request to the data layer
        /// </summary>
        protected IMediator Mediator { get; }

        /// <summary>
        /// Projection from response model to the given model 
        /// </summary>
        /// <param name="model"> Model from queries </param>
        /// <typeparam name="T"> Model to map to </typeparam>
        /// <returns></returns>
        protected IActionResult ProjectToResult<T>(BaseResponseModel model)
        {
            return !model.IsSuccess ? 
                ConvertToHttpResponse(model.ErrorCode, model.ErrorMessage) : 
                Ok(Mapper.Map<T>(model));
        }

        /// <summary>
        /// If there is no data to return. We just map to an OK result
        /// </summary>
        /// <param name="model"> The model to check. This will most often be a VoidObject </param>
        /// <returns> Ok if everything went well </returns>
        protected IActionResult OkResult(BaseResponseModel model)
        {
            return !model.IsSuccess ? 
                ConvertToHttpResponse(model.ErrorCode, model.ErrorMessage) : 
                Ok();
        }

        /// <summary>
        /// Converting internal error messages to http status codes
        /// </summary>
        /// <param name="errorCode"> The error code to map </param>
        /// <param name="message"> The message to generate the response with </param>
        /// <returns> The response with code and message </returns>
        /// <exception cref="Exception"> If we have forgotten to map a type of error code throw exception </exception>
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