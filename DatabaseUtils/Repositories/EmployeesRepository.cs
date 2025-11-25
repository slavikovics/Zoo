using System.Data;
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
        int? spouseId = null;
        if (model.Spouse is not null)
        {
            spouseId = model.Spouse.Id;
        }

        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            var getSpouseSql = "SELECT GetEmployeeSpouseId(@id)";
            var currentSpouseId =
                await connection.ExecuteScalarAsync<int?>(getSpouseSql, new { id = model.Id }, transaction);

            if (spouseId != currentSpouseId)
            {
                await connection.ExecuteAsync(
                    "CALL RemoveEmployeeSpouse(@employeeId)",
                    new { employeeId = model.Id }, transaction);
            }

            await connection.ExecuteAsync(
                "CALL UpdateEmployee(@id, @name, @birthdate::date, @phoneNumber, @maritalStatus)",
                new
                {
                    id = model.Id,
                    name = model.Name,
                    birthdate = model.BirthDate,
                    phoneNumber = model.PhoneNumber,
                    maritalStatus = model.MaritalStatus
                }, transaction);

            if (spouseId != currentSpouseId && spouseId is not null)
            {
                await connection.ExecuteAsync(
                    "CALL AddEmployeeSpouse(@employeeId, @spouseId)",
                    new { employeeId = model.Id, spouseId = spouseId.Value }, transaction);
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task Delete(int id)
    {
        await _deleteService.Delete<Employee>(id);
    }

    public async Task<int?> Create(Employee employee)
    {
        int? spouseId = employee.Spouse?.Id;

        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_name", employee.Name);
            parameters.Add("p_birthdate", employee.BirthDate.Date, DbType.Date);
            parameters.Add("p_phone_number", employee.PhoneNumber);
            parameters.Add("p_marital_status", employee.MaritalStatus);
            parameters.Add("new_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("CreateEmployee", parameters, commandType: CommandType.StoredProcedure,
                transaction: transaction);

            var newEmployeeId = parameters.Get<int?>("new_id");

            if (spouseId is not null && newEmployeeId is not null)
            {
                await connection.ExecuteAsync(
                    "CALL AddEmployeeSpouse(@employeeId, @spouseId)",
                    new { employeeId = newEmployeeId.Value, spouseId = spouseId.Value },
                    transaction: transaction);
            }

            transaction.Commit();
            return newEmployeeId;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
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

    public async Task RemoveSpouse(Employee employee)
    {
        var sql = "CALL RemoveEmployeeSpouse(@employeeId)";
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