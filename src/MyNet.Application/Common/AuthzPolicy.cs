
namespace MyNet.Application.Common
{
    public static class AuthzPolicy
    {
        // Identity Roles
        public const string ADMIN_ROLE = "Admin";
        public const string USER_ROLE = "User";

        // Policies
        public const string ADMIN_POLICY = "RequireAdminRole";
        public const string USER_POLICY = "RequireUserRole";
    }
}
