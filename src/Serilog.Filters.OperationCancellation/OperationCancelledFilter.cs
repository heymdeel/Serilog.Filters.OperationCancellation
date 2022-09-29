using Serilog.Core;
using Serilog.Events;
using System;

namespace Serilog.Filters.OperationCancellation
{
    public class OperationCancelledFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            if (logEvent.Exception is OperationCanceledException)
            {
                return false;
            }

            if (logEvent.Exception == null
                && logEvent.Properties.ContainsKey("SourceContext")
                && logEvent.Properties["SourceContext"].ToString() == "\"Microsoft.EntityFrameworkCore.Database.Command\""
                && logEvent.MessageTemplate.Text.StartsWith("Failed executing DbCommand"))
            {
                return false;
            }

            return true;
        }
    }
}
