using DatabaseUtils.Models;

namespace DatabaseUtils.Repositories;

public interface IAnimalsRepository : IRepository<Animal>
{
    Task AddVets(int animalId, List<Employee> vets);
    Task RemoveAllVets(int animalId);
    Task<IEnumerable<Employee>> GetAllVets(int animalId);
    Task<IEnumerable<Animal>> Search(string name, int? typeId);
    Task<int?> CreateWithVets(Animal animal, List<Employee> vets);
    Task UpdateWithVets(Animal model, List<Employee> vets);
}