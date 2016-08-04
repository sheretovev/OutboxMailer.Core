using Serilog.Core;
using Serilog.Events;

namespace Iquality.Shared.OutboxMailer.Core.Logging
{
    public class StandardLengthLevelName : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var newprop = $"[{logEvent.Level.ToString()}]";
            newprop = newprop.PadRight(14, '=');
            newprop += ">";
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(nameof(StandardLengthLevelName), newprop));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Tab", "".PadRight(36, ' ')));
        }
    }
}