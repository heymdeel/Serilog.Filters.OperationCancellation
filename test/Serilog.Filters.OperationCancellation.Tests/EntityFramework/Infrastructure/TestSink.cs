using Serilog.Core;
using Serilog.Events;

namespace Serilog.Filters.OperationCancellation.Tests;

public class TestInMemorySink : ILogEventSink
{
    public List<LogEvent> Events { get; set; } = new();

    public void Emit(LogEvent logEvent)
    {
        Events.Add(logEvent);
    }
}
