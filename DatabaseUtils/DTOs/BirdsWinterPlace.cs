using System.Runtime.InteropServices.JavaScript;

namespace DatabaseUtils.DTOs;

public class BirdsWinterPlace(int id, string name, DateTime departure, DateTime arrival) : IFieldNamesAvailable
{
    public int Id { get; set; } = id;

    public string CountryName { get; set; } = name;

    public DateTime Departure { get; set; } = departure;

    public DateTime Arrival { get; set; } = arrival;
    
    public List<string> GetAllFieldNames()
    {
        return [nameof(Id), nameof(CountryName), nameof(Departure), nameof(Arrival)];
    }
}