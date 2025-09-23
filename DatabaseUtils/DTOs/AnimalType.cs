namespace DatabaseUtils.DTOs;

public class AnimalType(int id, string name) : IFieldNamesAvailable
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;
    
    public List<string> GetAllFieldNames()
    {
        return [nameof(Id), nameof(Name)];
    }
}