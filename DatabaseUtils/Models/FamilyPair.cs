namespace DatabaseUtils.Models;

public class FamilyPair(
    string employeeName,
    DateTime employeeBirthDate,
    string employeePhoneNumber,
    string spouseName,
    DateTime spouseBirthDate,
    string spousePhoneNumber)
{
    public string EmployeeName { get; set; } = employeeName;

    public DateTime EmployeeBirthDate { get; set; } = employeeBirthDate;

    public string EmployeePhoneNumber { get; set; } = employeePhoneNumber;

    public string SpouseName { get; set; } = spouseName;

    public DateTime SpouseBirthDate { get; set; } = spouseBirthDate;

    public string SpousePhoneNumber { get; set; } = spousePhoneNumber;

    public override string ToString()
    {
        return $"Семейная пара {EmployeeName} - {SpouseName}";
    }
}