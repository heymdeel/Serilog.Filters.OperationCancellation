using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Serilog.Filters.OperationCancellation.Tests;

internal class DelayDbCommandInterceptor : DbCommandInterceptor
{
    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        await Task.Delay(5000, cancellationToken);

        return await ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }
}
