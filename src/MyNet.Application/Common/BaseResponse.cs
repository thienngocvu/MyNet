namespace MyNet.Application.Common
{
    public class BaseResponse
    {
        public required string ErrorCode { get; set; }
        public object? Data { get; set; }

        public static BaseResponse Error(string ErrorCode)
        {
            return new BaseResponse { ErrorCode = ErrorCode };
        }
    }
}
