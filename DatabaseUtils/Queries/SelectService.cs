using Dapper;
using DatabaseUtils.DTOs;
using Microsoft.Data.SqlClient;

namespace DatabaseUtils.Queries;

public class SelectService<T> : ISelectService<T>
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public SelectService(IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _connectionFactory = databaseConnectionFactory;
    }
    
    public async Task<IEnumerable<T>?> SelectAll(string tableName)
    {
        var selectQuery = $"SELECT * FROM {tableName}";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(selectQuery);
    }
    
    public async Task<IEnumerable<T>?> SelectById(string tableName, string idColumnName, int id)
    {
        var selectQuery = $@"SELECT * FROM {tableName} WHERE {idColumnName} = @id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(selectQuery, new {id});
    }
}