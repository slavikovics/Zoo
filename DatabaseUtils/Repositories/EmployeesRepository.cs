using Dapper;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace DatabaseUtils.Repositories;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly ISelectService _selectService;

    private readonly IDeleteService _deleteService;

    private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

    public EmployeesRepository(ISelectService selectService, IDeleteService deleteService,
        IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<IEnumerable<Employee>?> SelectAll()
    {
        return await _selectService.SelectAll<Employee>();
    }

    public async Task<IEnumerable<Employee>?> SelectById(int id)
    {
        return await _selectService.SelectById<Employee>(id);
    }

    public async Task Update(Employee model)
    {
        var sql = "CALL UpdateEmployee(@id, @name, @birthdate::date, @phoneNumber, @maritalStatus)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            id = model.Id,
            name = model.Name,
            birthdate = model.BirthDate,
            phoneNumber = model.PhoneNumber,
            maritalStatus = model.MaritalStatus
        });
    }

    public async Task Delete(int id)
    {
        await _deleteService.Delete<Employee>(id);
    }

    public async Task<int?> Create(Employee employee)
    {
        var create = @"SELECT CreateEmployee(@name, @birthdate::date, @phoneNumber, @maritalStatus)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        var newId = await connection.ExecuteScalarAsync<int>(create, new
        {
            employee.Name,
            employee.BirthDate,
            employee.PhoneNumber,
            employee.MaritalStatus
        });

        return newId;
    }

    public async Task AddSpouse(int employeeId, Employee spouse)
    {
        var sql = "CALL AddEmployeeSpouse(@employeeId, @spouseId)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            employeeId,
            spouseId = spouse.Id
        });
    }

    public async Task RemoveAllSpouses(Employee employee)
    {
        var sql = "CALL RemoveAllEmployeeSpouses(@employeeId)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            employeeId = employee.Id
        });
    }
    
    public async Task<int?> GetSpouseId(int employeeId)
    {
        var sql = "SELECT GetEmployeeSpouseId(@employeeId)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        var spouseId = await connection.ExecuteScalarAsync<int?>(sql, new
        {
            employeeId
        });

        return spouseId;
    }

}