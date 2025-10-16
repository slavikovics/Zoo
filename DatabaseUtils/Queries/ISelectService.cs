namespace DatabaseUtils.Queries;

public interface ISelectService
{
    Task<IEnumerable<T>?> SelectAll<T>(string tableName) where T : class;

    Task<IEnumerable<T>?> SelectById<T>(string tableName, string idColumnName, int id) where T : class;
}