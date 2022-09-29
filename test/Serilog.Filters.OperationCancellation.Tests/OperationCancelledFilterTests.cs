using FluentAssertions;
using Serilog.Events;
using Serilog.Parsing;

namespace Serilog.Filters.OperationCancellation.Tests
{
    public class OperationCancelledFilterTests
    {
        [Fact]
        public void OperationCancelledFilter_ShouldFilterOutOperationCancelledExceptions()
        {
            // Arrange
            var logEvent = new LogEvent(
                timestamp: DateTimeOffset.UtcNow,
                level: LogEventLevel.Error,
                exception: new OperationCanceledException(),
                messageTemplate: new MessageTemplate("test", Array.Empty<MessageTemplateToken>()),
                properties: Array.Empty<LogEventProperty>());

            var filter = new OperationCancelledFilter();

            // Act
            bool result = filter.IsEnabled(logEvent);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void OperationCancelledFilter_ShouldFilterOutEntityFrameworkCancellationLogs()
        {
            // Arrange
            var properties = new List<LogEventProperty>()
            {
                new("SourceContext", new ScalarValue("Microsoft.EntityFrameworkCore.Database.Command"))
            };

            var logEvent = new LogEvent(
                timestamp: DateTimeOffset.UtcNow,
                level: LogEventLevel.Error,
                exception: null,
                messageTemplate: new MessageTemplate("Failed executing DbCommand (\"20\"ms) [Parameters=", Array.Empty<MessageTemplateToken>()),
                properties: properties);

            var filter = new OperationCancelledFilter();

            // Act
            bool result = filter.IsEnabled(logEvent);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void OperationCancelledFilter_ShouldAllowAnyOtherError()
        {
            // Arrange
            var logEvent = new LogEvent(
                timestamp: DateTimeOffset.UtcNow,
                level: LogEventLevel.Error,
                exception: new ArgumentNullException("testArgument", "some message"),
                messageTemplate: new MessageTemplate("test error message", Array.Empty<MessageTemplateToken>()),
                properties: Array.Empty<LogEventProperty>());

            var filter = new OperationCancelledFilter();

            // Act
            bool result = filter.IsEnabled(logEvent);

            // Assert
            result.Should().BeTrue();
        }
    }
}
