using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog.Extensions.Logging;

namespace Serilog.Filters.OperationCancellation.Tests;

internal class TestContext : DbContext
{
    private readonly ILogger _logger;

    private readonly SqliteConnection _connection;

    public DbSet<TestEntity> TestEntities { get; set; }

    public TestContext(ILogger logger, SqliteConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseLoggerFactory(new SerilogLoggerFactory(_logger))
            .UseSqlite(_connection)
            .AddInterceptors(new DelayDbCommandInterceptor());
    }
}
