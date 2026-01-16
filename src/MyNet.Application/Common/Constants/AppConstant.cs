namespace MyNet.Application.Common.Constants
{
    public static class AppConstant
    {
        public const string AppConnectionStringName = "DbConnection";
        public const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ss:fff";
        public const string AppApiCorsPolicy = "api";

        public static class StatusCodeResponse
        {
            public const int SUCCESS = 500;
            public const int UNAUTHORIZED = 401;
            public const int VALIDATION = 422;
            public const int FOR_BIDDEN = 403;
            public const int BAD_REQUEST = 400;
            public const int SERVE_RERROR = 500;
        }

        public const string DELETE_SUCCESS_MESSAGE = "Delete success";
        public const string EDIT_SUCCESS_MESSAGE = "Edit success";
    }
}
