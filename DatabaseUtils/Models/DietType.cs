namespace DatabaseUtils.Models;

public class DietType(int id, string type)
{
    public int Id { get; set; } = id;

    public string Type { get; set; } = type;
    
    public override string ToString()
    {
        return $"{Type}, ID: {Id}";
    }
}