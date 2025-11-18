using Dapper;
using DatabaseUtils.TableNameResolver;

namespace DatabaseUtils.Queries;

public class DeleteService : IDeleteService
{
    private readonly IDatabaseConnectionFactory _connectionFactory;
    private readonly ITableNameResolver _tableNameResolver;

    public DeleteService(IDatabaseConnectionFactory databaseConnectionFactory, ITableNameResolver tableNameResolver)
    {
        _connectionFactory = databaseConnectionFactory;
        _tableNameResolver = tableNameResolver;
    }

    public async Task Delete<T>(int id, string idColumnName = "Id") where T : class
    {
        var deleteQuery = $@"DELETE FROM {_tableNameResolver.ResolveTableName<T>()} WHERE {idColumnName} = @id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(deleteQuery, new { id });
    }
}