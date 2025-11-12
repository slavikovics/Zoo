using Dapper;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace DatabaseUtils.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private readonly ISelectService _selectService;

    private readonly IDeleteService _deleteService;

    private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

    public AnimalsRepository(ISelectService selectService, IDeleteService deleteService,
        IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<IEnumerable<Animal>?> SelectAll()
    {
        return await _selectService.SelectAll<Animal>();
    }

    public async Task<IEnumerable<Animal>?> SelectById(int id)
    {
        return await _selectService.SelectById<Animal>(id);
    }

    public async Task Update(Animal model)
    {
        var sql = @"CALL UpdateAnimal(
        @animalId,
        @name,
        @typeId,
        @birthdate::date,
        @sex,
        @winterPlaceId,
        @reptileInfoId,
        @dietId,
        @habitatZoneId,
        @caretakerId)";

        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(sql, new
        {
            animalId = model.Id,
            model.Name,
            model.TypeId,
            model.BirthDate,
            model.Sex,
            model.WinterPlaceId,
            model.ReptileInfoId,
            model.DietId,
            model.HabitatZoneId,
            model.CaretakerId
        });
    }

    public async Task Delete(int id)
    {
        await _deleteService.Delete<Animal>(id);
    }

    public async Task<int?> Create(Animal animal)
    {
        var create =
            @"SELECT CreateAnimal(@name, @typeId, @birthdate::date, @sex, @winterPlaceId, @reptileInfoId, @dietId, @habitatZoneId, @caretakerId)";
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        var newId = await connection.ExecuteScalarAsync<int>(create, new
        {
            animal.Name,
            animal.TypeId,
            animal.BirthDate,
            animal.Sex,
            animal.WinterPlaceId,
            animal.ReptileInfoId,
            animal.DietId,
            animal.HabitatZoneId,
            animal.CaretakerId
        });

        return newId;
    }

    public async Task AddVets(int animalId, List<Employee> vets)
    {
        var vetIds = vets.Select(v => v.Id).ToArray();
        var sql = "CALL AddVetArrayToAnimal(@animalId, @vetIds)";

        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, new
        {
            animalId,
            vetIds
        });
    }
}