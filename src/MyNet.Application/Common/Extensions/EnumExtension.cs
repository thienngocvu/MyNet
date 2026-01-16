using System.ComponentModel;

namespace MyNet.Application.Common.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumDescriptions<T>(this T e) where T : IConvertible
        {
            if (!(e is Enum))
                return string.Empty;

            var field = e?.GetType().GetField(e.ToString() ?? string.Empty);
            if (field == null) return string.Empty;

            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var description = customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : string.Empty;

            return description;
        }
    }
}
