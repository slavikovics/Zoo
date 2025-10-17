using System.Runtime.InteropServices.JavaScript;

namespace DatabaseUtils.DTOs;

public class BirdsWinterPlace(int id, string countryName, DateTime departure, DateTime arrival)
{
    public int Id { get; set; } = id;

    public string CountryName { get; set; } = countryName;

    public DateTime Departure { get; set; } = departure;

    public DateTime Arrival { get; set; } = arrival;
    
    public override string ToString()
    {
        return $"{CountryName}, отправление: {Departure}, прибытие: {Arrival}";
    }
}