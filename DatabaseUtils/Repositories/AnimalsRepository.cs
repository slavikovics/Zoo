using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace DatabaseUtils.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private readonly ISelectService _selectService;
    
    private readonly IDeleteService _deleteService;
    
    public AnimalsRepository(ISelectService selectService, IDeleteService deleteService)
    {
        _selectService = selectService;
        _deleteService = deleteService;
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
        throw new NotImplementedException();
    }

    public async Task Delete(int id)
    {
        await _deleteService.Delete<Animal>(id);
    }

    public async Task Create(int id)
    {
        throw new NotImplementedException();
    }
}