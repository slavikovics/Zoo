namespace DatabaseUtils.TableNameResolver;

public class TableNameResolver : ITableNameResolver
{
    private readonly Dictionary<string, string> _tableNameResolverDictionary;
    private readonly string _connectionString;

    public TableNameResolver(Dictionary<string, string> tableNameResolverDictionary, string connectionString)
    {
        _tableNameResolverDictionary = tableNameResolverDictionary;
        _connectionString = connectionString;
    }

    public string? ResolveTableName<T>() where T : class
    {
        if (_tableNameResolverDictionary.ContainsKey(typeof(T).Name))
            return _tableNameResolverDictionary[typeof(T).Name];

        return null;
    }

    public string ResolveConnectionString()
    {
        return _connectionString;
    }
}