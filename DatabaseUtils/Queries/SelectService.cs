using Dapper;
using DatabaseUtils.TableNameResolver;

namespace DatabaseUtils.Queries;

public class SelectService : ISelectService
{
    private readonly IDatabaseConnectionFactory _connectionFactory;
    
    private readonly ITableNameResolver _tableNameResolver;

    public SelectService(IDatabaseConnectionFactory databaseConnectionFactory, ITableNameResolver tableNameResolver)
    {
        _connectionFactory = databaseConnectionFactory;
        _tableNameResolver = tableNameResolver;
    }
    
    public async Task<IEnumerable<T>?> SelectAll<T>() where T : class
    {
        var selectQuery = $"SELECT * FROM {_tableNameResolver.ResolveTableName<T>()}";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(selectQuery);
    }
    
    public async Task<IEnumerable<T>?> SelectById<T>(int id, string idColumnName = "Id") where T : class
    {
        var selectQuery = $@"SELECT * FROM {_tableNameResolver.ResolveTableName<T>()} WHERE {idColumnName} = @id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(selectQuery, new {id});
    }
}