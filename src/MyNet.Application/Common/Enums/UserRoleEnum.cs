using System.ComponentModel;

namespace MyNet.Application.Common.Enums
{
    [Flags]
    public enum UserRoleEnum : short
    {
        [Description(AuthzPolicy.ADMIN_ROLE)]
        ADMIN_USER = 1,
        [Description(AuthzPolicy.USER_ROLE)]
        USER_GENERAL = 0,
    }
}
