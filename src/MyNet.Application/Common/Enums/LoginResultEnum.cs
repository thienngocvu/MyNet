using System.ComponentModel;

namespace MyNet.Application.Common.Enums
{
    [Flags]
    public enum LoginResultEnum : short
    {
        [Description("0")]
        LOGIN_SUCCESSFULLY = 0,
        [Description("-1")]
        LOGIN_FAILED = -1,
    }
}
