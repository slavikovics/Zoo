namespace DatabaseUtils.DTOs;

public class Employee(int id, string name, DateTime birthDate, string phoneNumber, 
    string maritalStatus, int? marriedWith) : IFieldNamesAvailable
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;

    public DateTime BirthDate { get; set; } = birthDate;

    public string PhoneNumber { get; set; } = phoneNumber;

    public string MaritalStatus { get; set; } = maritalStatus;

    public int? MarriedWith { get; set; } = marriedWith;
    
    public List<string> GetAllFieldNames()
    {
        return [nameof(Id), nameof(Name), nameof(BirthDate), nameof(PhoneNumber), nameof(MaritalStatus), 
            nameof(MarriedWith)];
    }
}