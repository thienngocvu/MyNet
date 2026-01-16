namespace MyNet.Application.Common.Exceptions
{
    public class BaseException : Exception
    {
        public string? ErrorCode { get; set; }
        public string? DetailCode { get; set; }
        public IDictionary<string, object>? ExtraData { get; set; }

        protected BaseException() : base() { }

        protected BaseException(string errorCode) : base(errorCode)
        {
            ErrorCode = errorCode;
            DetailCode = string.IsNullOrEmpty(errorCode) ? string.Empty : errorCode;
        }

        protected BaseException(string? errorCode, string? detailCode) : base(errorCode)
        {
            ErrorCode = errorCode;
            DetailCode = detailCode;
        }

        protected BaseException(string? errorCode, string? detailCode, IDictionary<string, object>? extraData) : base(errorCode)
        {
            ErrorCode = errorCode;
            DetailCode = detailCode;
            ExtraData = extraData;
        }

        protected BaseException(string? errorCode, string? detailCode, IDictionary<string, object>? extraData, Exception? innerException = null) : base(errorCode, innerException)
        {
            ErrorCode = errorCode;
            DetailCode = detailCode;
            ExtraData = extraData;
        }
    }
}
