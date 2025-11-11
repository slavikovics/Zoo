namespace DatabaseUtils.Models;

public class BirdsWinterPlace(int? id, string? countryName, DateTime departure, DateTime arrival)
{
    public int? Id { get; set; } = id;

    public string? CountryName { get; set; } = countryName;

    public DateTime Departure { get; set; } = departure;

    public DateTime Arrival { get; set; } = arrival;
    
    public override string ToString()
    {
        if (Id is not null) return $"{CountryName}, отправление: {Departure}, прибытие: {Arrival}";
        return $"Без места зимовки";
    }

    public static BirdsWinterPlace Empty()
    {
        return new BirdsWinterPlace(null, null, DateTime.MinValue, DateTime.MinValue);
    }
}