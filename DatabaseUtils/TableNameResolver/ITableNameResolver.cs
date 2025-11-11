namespace DatabaseUtils.TableNameResolver;

public interface ITableNameResolver
{
    string? ResolveTableName<T>() where T : class;
}