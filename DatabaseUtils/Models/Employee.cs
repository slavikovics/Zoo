namespace DatabaseUtils.Models;

public class Employee(int? id, string name, DateTime birthDate, string phoneNumber, 
    string maritalStatus)
{
    public int? Id { get; set; } = id;

    public string Name { get; set; } = name;

    public DateTimeOffset BirthDate { get; set; } = birthDate.ToUniversalTime();

    public string PhoneNumber { get; set; } = phoneNumber;

    public string MaritalStatus { get; set; } = maritalStatus;
    
    public override string ToString()
    {
        if (Id is null) return "Нет сотрудника";
        return $"{Name}, дата рождения: {BirthDate}, номер телефона: {PhoneNumber}, семейное положение: {MaritalStatus}";
    }
    
    public static Employee Empty()
    {
        return new Employee(null, "", DateTime.MinValue, "", "");
    }
}