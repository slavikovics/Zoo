namespace DatabaseUtils.Queries;

public interface IDeleteService
{
    public Task Delete<T>(int id, string idColumnName = "Id") where T : class;
}