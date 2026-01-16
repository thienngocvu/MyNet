using MyNet.Application.Common.Constants;

namespace MyNet.Application.Common.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(
            string? detailCode = null,
            IDictionary<string, object>? payload = null,
            Exception? innerException = null)
        : base(ExceptionErrorCodeConstant.ERROR_BAD_REQUEST_EXCEPTION, detailCode, payload, innerException) { }
    }
}
