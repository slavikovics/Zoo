using Dapper;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace DatabaseUtils.Repositories;

public class DietsRepository : IDietsRepository
{
    private readonly ISelectService _selectService;

    private readonly IDeleteService _deleteService;

    private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

    public DietsRepository(ISelectService selectService, IDeleteService deleteService,
        IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<IEnumerable<Diet>?> SelectAll()
    {
        return await _selectService.SelectAll<Diet>();
    }

    public async Task<IEnumerable<Diet>?> SelectById(int id)
    {
        return await _selectService.SelectById<Diet>(id);
    }

    public async Task Update(Diet model)
    {
        var sql = "CALL UpdateDiet(@id, @name, @typeId, @description)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            id = model.Id,
            name = model.Name,
            typeId = model.TypeId,
            description = model.Description
        });
    }

    public async Task Delete(int id)
    {
        await _deleteService.Delete<Diet>(id);
    }

    public async Task<int?> Create(Diet diet)
    {
        var sql = "CALL CreateDiet(@name, @typeId, @description)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            name = diet.Name,
            typeId = diet.TypeId,
            description = diet.Description
        });

        return null;
    }
}