namespace DatabaseUtils.DTOs;

public class ReptileInfo(int id, decimal normalTemperature, DateTime sleepStart, DateTime sleepEnd) : IFieldNamesAvailable
{
    public int Id { get; set; } = id;
    
    public decimal NormalTemperature { get; set; } = normalTemperature;
    
    public DateTime SleepStart { get; set; } = sleepStart;

    public DateTime SleepEnd { get; set; } = sleepEnd;
    
    public List<string> GetAllFieldNames()
    {
        return [nameof(Id), nameof(NormalTemperature), nameof(SleepStart), nameof(SleepEnd)];
    }
}