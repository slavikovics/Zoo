using DatabaseUtils.Models;

namespace DatabaseUtils.Repositories;

public interface IAnimalsRepository : IRepository<Animal>
{
    Task AddVets(int animalId, List<Employee> vets);
}