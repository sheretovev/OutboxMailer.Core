using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Iquality.Shared.OutboxMailer.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToLog(this object objectToGetStateOf)
        {            
            if (objectToGetStateOf is string
                || objectToGetStateOf is int
                || objectToGetStateOf is double
                || objectToGetStateOf is float
                || objectToGetStateOf is bool
                || objectToGetStateOf is Guid
                || objectToGetStateOf is DateTime
                || objectToGetStateOf is Guid)
            {
                return objectToGetStateOf.ToString();  // return string value if primitive type
            }
            if (objectToGetStateOf is IEnumerable)
            {
                return string.Concat((objectToGetStateOf as IEnumerable<object>).Select(x => x.ToLog()));
            }
            var builder = new StringBuilder();
            builder.AppendLine("{ ");
            foreach (var property in objectToGetStateOf.GetType().GetProperties())
            {
                var value = property.GetValue(objectToGetStateOf, null);

                builder.Append(property.Name)
                .Append("=")
                .Append((value?.ToLog() ?? "null"))
                .AppendLine();
            }
            builder.AppendLine(" }");
            return builder.ToString(); // return object properties if reference type
        }
    }
}
