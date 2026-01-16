using Microsoft.AspNetCore.Authorization;

namespace MyNet.Application.Common.Attributes
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "PERMISSION_";
        public const string SEPARATOR = "_";

        public HasPermissionAttribute(string function, string action)
        {
            Policy = $"{POLICY_PREFIX}{function}{SEPARATOR}{action}";
        }
    }
}
