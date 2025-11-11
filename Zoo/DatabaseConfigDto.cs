using System.Collections.Generic;

namespace Zoo;

public class DatabaseConfigDto(string connectionString, Dictionary<string, string> tableNames)
{
    public string ConnectionString { get; set; } = connectionString;

    public Dictionary<string, string> TableNames { get; set; } = tableNames;
}