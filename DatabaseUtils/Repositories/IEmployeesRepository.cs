using DatabaseUtils.Models;

namespace DatabaseUtils.Repositories;

public interface IEmployeesRepository : IRepository<Employee>
{
    Task AddSpouse(int employeeId, Employee spouse);
    Task RemoveAllSpouses(Employee employee);
}