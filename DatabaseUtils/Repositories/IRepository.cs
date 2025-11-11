namespace DatabaseUtils.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>?> SelectAll();
    
    Task<IEnumerable<T>?> SelectById(int id);
    
    Task Update(T model);
    
    Task Delete(int id);
    
    Task<int?> Create(T model);
}