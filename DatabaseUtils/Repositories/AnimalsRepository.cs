using System.Data;
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

    public async Task UpdateWithVets(Animal model, List<Employee> vets)
    {
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(
                "CALL RemoveAllVetsFromAnimal(@animalId)",
                new { animalId = model.Id },
                transaction: transaction);

            await connection.ExecuteAsync(
                @"CALL UpdateAnimal(
                @animalId,
                @name,
                @typeId,
                @birthdate::date,
                @sex,
                @winterPlaceId,
                @reptileInfoId,
                @dietId,
                @habitatZoneId,
                @caretakerId)",
                new
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
                },
                transaction: transaction);

            if (vets.Any())
            {
                var vetIds = vets.Select(v => v.Id).ToArray();
                await connection.ExecuteAsync(
                    "CALL AddVetArrayToAnimal(@animalId, @vetIds)",
                    new { animalId = model.Id, vetIds },
                    transaction: transaction);
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
        await _deleteService.Delete<Animal>(id);
    }

    public async Task<int?> Create(Animal animal)
    {
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        var parameters = new DynamicParameters();
        parameters.Add("p_name", animal.Name);
        parameters.Add("p_type_id", animal.TypeId);
        parameters.Add("p_birthdate", animal.BirthDate, DbType.Date);
        parameters.Add("p_sex", animal.Sex);
        parameters.Add("p_winter_place_id", animal.WinterPlaceId);
        parameters.Add("p_reptile_info_id", animal.ReptileInfoId);
        parameters.Add("p_diet_id", animal.DietId);
        parameters.Add("p_habitat_zone_id", animal.HabitatZoneId);
        parameters.Add("p_caretaker_id", animal.CaretakerId);
        parameters.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

        await connection.ExecuteAsync("CreateAnimal", parameters, commandType: CommandType.StoredProcedure);
        return parameters.Get<int?>("p_id");
    }

    public async Task<int?> CreateWithVets(Animal animal, List<Employee> vets)
    {
        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_name", animal.Name);
            parameters.Add("p_type_id", animal.TypeId);
            parameters.Add("p_birthdate", animal.BirthDate, DbType.Date);
            parameters.Add("p_sex", animal.Sex);
            parameters.Add("p_winter_place_id", animal.WinterPlaceId);
            parameters.Add("p_reptile_info_id", animal.ReptileInfoId);
            parameters.Add("p_diet_id", animal.DietId);
            parameters.Add("p_habitat_zone_id", animal.HabitatZoneId);
            parameters.Add("p_caretaker_id", animal.CaretakerId);
            parameters.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

            await connection.ExecuteAsync("CreateAnimal", parameters, commandType: CommandType.StoredProcedure,
                transaction: transaction);

            var animalId = parameters.Get<int?>("p_id");
            if (animalId.HasValue && vets.Any())
            {
                var vetIds = vets.Select(v => v.Id).ToArray();
                await connection.ExecuteAsync(
                    "CALL AddVetArrayToAnimal(@animalId, @vetIds)",
                    new { animalId = animalId.Value, vetIds },
                    transaction: transaction);
            }

            transaction.Commit();
            return animalId;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
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

    public async Task RemoveAllVets(int animalId)
    {
        var sql = "CALL RemoveAllVetsFromAnimal(@animalId)";

        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, new
        {
            animalId
        });
    }

    public async Task<IEnumerable<Employee>> GetAllVets(int animalId)
    {
        var sql = "SELECT * FROM GetAnimalVetsFullInfo(@animalId)";

        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Employee>(sql, new
        {
            animalId
        });
    }

    public async Task<IEnumerable<Animal>> Search(string name, int? typeId)
    {
        var sql =
            @"SELECT * FROM ANIMALS WHERE (Name LIKE '%' || @name || '%') AND (TypeId = @typeId)";

        using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

        return await connection.QueryAsync<Animal>(sql, new
        {
            name,
            typeId
        });
    }
}