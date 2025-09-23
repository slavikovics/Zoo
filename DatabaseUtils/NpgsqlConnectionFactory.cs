using System.Data;
using Npgsql;

namespace DatabaseUtils;

public class NpgsqlConnectionFactory(string connectionString) : IDatabaseConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}