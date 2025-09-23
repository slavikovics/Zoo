using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseUtils.DTOs;

public class Diet(int id, string name, int typeId, string? description) : IFieldNamesAvailable
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;

    public int TypeId { get; set; } = typeId;

    public string? Description { get; set; } = description;
    
    public List<string> GetAllFieldNames()
    {
        return [nameof(Id), nameof(Name), nameof(TypeId), nameof(Description)];
    }
}