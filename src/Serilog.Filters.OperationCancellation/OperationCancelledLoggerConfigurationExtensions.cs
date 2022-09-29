using Serilog.Configuration;

namespace Serilog.Filters.OperationCancellation
{
    public static class OperationCancelledLoggerConfigurationExtensions
    {
        public static LoggerConfiguration ByExcludingOperationCancelledExceptions(this LoggerFilterConfiguration loggerFilterConfiguration)
        {
            return loggerFilterConfiguration.With<OperationCancelledFilter>();
        }
    }
}
