using System.Reflection;
using System.Text;

namespace Iquality.Shared.OutboxMailer.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToLog(this object objectToGetStateOf)
        {
            var builder = new StringBuilder();
            foreach (var property in objectToGetStateOf.GetType().GetProperties())
            {
                var value = property.GetValue(objectToGetStateOf, null);

                builder.Append(property.Name)
                .Append(" = ")
                .Append((value ?? "null"))
                .AppendLine();
            }
            return builder.ToString();
        }
    }
}
