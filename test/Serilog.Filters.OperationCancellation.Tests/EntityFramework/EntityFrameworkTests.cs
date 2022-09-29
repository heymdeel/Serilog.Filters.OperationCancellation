using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Filters.OperationCancellation.Tests;

public class EntityFrameworkTests
{
    [Fact]
    public async void DbContext_ShouldNotLogErrors_WhenCancellationTokenIsTriggered()
    {
        // Arrange
        var sink = new TestInMemorySink();
        var logger = new LoggerConfiguration()
            .Filter.ByExcludingOperationCancelledExceptions()
            .WriteTo.Sink(sink)
            .CreateLogger();

        var context = CreateTestContext(logger);
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        try
        {
            var databaseTask = context.TestEntities.ToListAsync(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            await databaseTask;
        }
        catch (Exception) { }

        // Assert
        sink.Events.Should().NotContain(eventLog => eventLog.Level == LogEventLevel.Error);
    }

    private static TestContext CreateTestContext(Logger logger)
    {
        var connection = new SqliteConnection("Data Source=LanguageHandlerTest;Mode=Memory;Cache=Shared");
        connection.Open();

        var context = new TestContext(logger, connection);
        context.Database.EnsureCreated();
        return context;
    }
}
