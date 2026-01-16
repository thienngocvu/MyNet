using MyNet.Application.Common.Constants;

namespace MyNet.Application.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(
            string? detailCode = null,
            IDictionary<string, object>? payload = null,
            Exception? innerException = null)
        : base(ExceptionErrorCodeConstant.ERROR_ENTITY_NOTFOUND, detailCode, payload, innerException) { }
    }
}
