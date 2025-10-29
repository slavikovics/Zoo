namespace DatabaseUtils.Queries;

public interface IDeleteService
{
    public Task Delete(int id, string tableName, string idColumnName);
}