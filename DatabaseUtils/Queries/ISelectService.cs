namespace DatabaseUtils.Queries;

public interface ISelectService
{
    Task<IEnumerable<T>?> SelectAll<T>() where T : class;

    Task<IEnumerable<T>?> SelectById<T>(int id, string idColumnName = "Id") where T : class;
}