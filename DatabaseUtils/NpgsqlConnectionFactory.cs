using System.Data;
using Npgsql;

namespace DatabaseUtils;

public class NpgsqlConnectionFactory(string connectionString) : IDatabaseConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}