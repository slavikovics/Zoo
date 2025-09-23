using System.Data;

namespace DatabaseUtils;

public interface IDatabaseConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}