namespace DatabaseUtils.DTOs;

public class Animal(int id, string name, int typeId, DateTime? birthDate, string sex, 
    int? winterPlaceId, int? reptileInfoId, int dietId, int habitatZoneId, int vet, int caretakerId)
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;

    public int TypeId { get; set; } = typeId;
    
    //
    public List<int> AvailableTypes { get; set; } = [1, 2, 3];
    //
    
    public DateTimeOffset? BirthDate { get; set; } =  birthDate;

    public string Sex { get; set; } = sex;
    
    public int? WinterPlaceId { get; set; } = winterPlaceId;
    
    public int? ReptileInfoId { get; set; } = reptileInfoId;
    
    public int DietId { get; set; } = dietId;

    public int HabitatZoneId { get; set; } = habitatZoneId;

    public int Vet { get; set; } = vet;

    public int CaretakerId { get; set; } = caretakerId;
}