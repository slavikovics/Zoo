namespace DatabaseUtils.Queries;

public interface ISelectService<T>
{
    Task<IEnumerable<T>?> SelectAll(string tableName);

    Task<IEnumerable<T>?> SelectById(string tableName, string idColumnName, int id);
}