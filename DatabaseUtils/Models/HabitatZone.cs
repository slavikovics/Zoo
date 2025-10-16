namespace DatabaseUtils.DTOs;

public class HabitatZone(int id, string name, string description)
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;
    
    public override string ToString()
    {
        return $"{Name}, описание: {Description}";
    }
}