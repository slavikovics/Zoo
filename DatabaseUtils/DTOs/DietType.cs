namespace DatabaseUtils.DTOs;

public class DietType(int id, string type) : IFieldNamesAvailable
{
    public int Id { get; set; } = id;

    public string Type { get; set; } = type;
    
    public List<string> GetAllFieldNames()
    {
        return [nameof(Id), nameof(Type)];
    }
}