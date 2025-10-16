using Dapper;
using DatabaseUtils.DTOs;
using Microsoft.Data.SqlClient;

namespace DatabaseUtils.Queries;

public class SelectService : ISelectService
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public SelectService(IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _connectionFactory = databaseConnectionFactory;
    }
    
    public async Task<IEnumerable<T>?> SelectAll<T>(string tableName) where T : class
    {
        var selectQuery = $"SELECT * FROM {tableName}";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(selectQuery);
    }
    
    public async Task<IEnumerable<T>?> SelectById<T>(string tableName, string idColumnName, int id) where T : class
    {
        var selectQuery = $@"SELECT * FROM {tableName} WHERE {idColumnName} = @id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(selectQuery, new {id});
    }
}