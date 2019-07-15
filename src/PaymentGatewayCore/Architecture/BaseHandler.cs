using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGatewayCore.Model;

namespace PaymentGatewayCore.Architecture
{
    /// <summary>
    /// Base request handler. Middleware for my commands/queries
    /// </summary>
    /// <typeparam name="TRequest"> The command/query to handle</typeparam>
    /// <typeparam name="TResponse"> The response from the command/query </typeparam>
    public abstract class BaseRequestHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse> where TRequest
        : IRequest<TResponse>
        where TResponse : BaseResponseModel, new()
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        public TResponse Error(ErrorCode errorCode, string errorMessage)
        {
            return new TResponse
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }

        protected TResponse Success()
        {
            return new TResponse
            {
                ErrorCode = ErrorCode.None
            };
        }

        protected TResponse Success(TResponse result)
        {
            result.ErrorCode = ErrorCode.None;
            return result;
        }
    }
}