namespace DatabaseUtils.DTOs;

public class AnimalType(int id, string name)
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;
}