using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HotelManagement.WebApp.Helpers
{

    public static class EnumExtensions
    {
        public static string ToDisplayName(this Enum value)
        {
            var member = value.GetType()
                              .GetMember(value.ToString())
                              .FirstOrDefault();

            return member?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? value.ToString();
        }
    }

}
