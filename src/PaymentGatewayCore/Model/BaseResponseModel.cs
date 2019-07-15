namespace PaymentGatewayCore.Model
{
    /// <summary>
    /// The base response model of the commands/queries
    ///
    /// Contains information about the request and whether it was a success or not
    /// </summary>
    public class BaseResponseModel
    {
        public bool IsSuccess => ErrorCode == ErrorCode.None;
        public string ErrorMessage { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }

    public enum ErrorCode
    {
        None = 0,
        NoAccess = 1,
        NotFound = 2,
        UnexpectedError = 3
    }
}