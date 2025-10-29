using Dapper;

namespace DatabaseUtils.Queries;

public class DeleteService : IDeleteService
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public DeleteService(IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _connectionFactory = databaseConnectionFactory;
    }

    public async Task Delete(int id, string tableName, string idColumnName = "Id")
    {
        var deleteQuery = $@"DELETE FROM {tableName} WHERE {idColumnName} = @id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(deleteQuery, new {id});
    }
}