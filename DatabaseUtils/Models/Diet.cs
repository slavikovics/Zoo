using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseUtils.DTOs;

public class Diet(int? id, string? name, int typeId, string? description)
{
    public int? Id { get; set; } = id;

    public string? Name { get; set; } = name;

    public int TypeId { get; set; } = typeId;

    public string? Description { get; set; } = description;
    
    public override string ToString()
    {
        if (Id is not null) return $"{Name}, тип: {TypeId}, описание: {Description}";
        return "Без диеты";
    }

    public static Diet Empty()
    {
        return new Diet(null, null, 0, null);
    }
}