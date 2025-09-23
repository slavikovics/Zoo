using Dapper;
using DatabaseUtils.DTOs;
using Microsoft.Data.SqlClient;

namespace DatabaseUtils.Queries;

public class Select<T> where T : IFieldNamesAvailable
{
    private string? _selectQuery;

    private readonly string _tableName;

    public Select(string tableName)
    {
        _tableName = tableName;
        PrebuildQuery();
    }

    private void PrebuildQuery()
    {
        _selectQuery = @"SELECT * FROM {_tableName}";
    }
    
    public async Task<IEnumerable<T>?> SelectAll()
    {
        if (_selectQuery == null) return null;
        await using var connection = new SqlConnection("");
        return await connection.QueryAsync<T>(_selectQuery);
    }
}