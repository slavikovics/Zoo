namespace DatabaseUtils.Models;

public class ReptileInfo(int? id, decimal normalTemperature, DateTime sleepStart, DateTime sleepEnd)
{
    public int? Id { get; set; } = id;
    
    public decimal NormalTemperature { get; set; } = normalTemperature;
    
    public DateTime SleepStart { get; set; } = sleepStart;

    public DateTime SleepEnd { get; set; } = sleepEnd;
    
    public override string ToString()
    {
        if (Id is null) return "Без дополнительной информации";
        return $"Температура: {NormalTemperature}, начало спячки: {SleepStart}, конец спячки: {SleepEnd}";
    }

    public static ReptileInfo Empty()
    {
        return new ReptileInfo(null, 0, DateTime.MinValue, DateTime.MinValue);
    }
}