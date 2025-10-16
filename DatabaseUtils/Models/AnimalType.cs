namespace DatabaseUtils.DTOs;

public class AnimalType(int id, string name)
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;
    
    public override string ToString()
    {
        return $"{Name}, ID: {Id}";
    }
}