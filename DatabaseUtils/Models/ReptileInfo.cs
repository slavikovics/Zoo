namespace DatabaseUtils.DTOs;

public class ReptileInfo(int id, decimal normalTemperature, DateTime sleepStart, DateTime sleepEnd)
{
    public int Id { get; set; } = id;
    
    public decimal NormalTemperature { get; set; } = normalTemperature;
    
    public DateTime SleepStart { get; set; } = sleepStart;

    public DateTime SleepEnd { get; set; } = sleepEnd;
    
    public override string ToString()
    {
        return $"Температура: {NormalTemperature}, начало спячки: {SleepStart}, конец спячки: {SleepEnd}";
    }
}