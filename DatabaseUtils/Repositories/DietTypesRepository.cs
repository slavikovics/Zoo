using Dapper;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace DatabaseUtils.Repositories;

public class DietTypesRepository : IDietTypesRepository
{
    private readonly ISelectService _selectService;
    private readonly IDeleteService _deleteService;
    private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

    public DietTypesRepository(ISelectService selectService, IDeleteService deleteService,
        IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<IEnumerable<DietType>?> SelectAll()
    {
        return await _selectService.SelectAll<DietType>();
    }

    public async Task<IEnumerable<DietType>?> SelectById(int id)
    {
        return await _selectService.SelectById<DietType>(id);
    }

    public async Task Update(DietType model)
    {
        var sql = "CALL UpdateDietType(@id, @type)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            id = model.Id,
            type = model.Type
        });
    }

    public async Task Delete(int id)
    {
        await _deleteService.Delete<DietType>(id);
    }

    public async Task<int?> Create(DietType dietType)
    {
        var sql = "CALL CreateDietType(@type)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            type = dietType.Type
        });

        return null;
    }
}