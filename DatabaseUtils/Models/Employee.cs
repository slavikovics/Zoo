namespace DatabaseUtils.DTOs;

public class Employee(int id, string name, DateTime birthDate, string phoneNumber, 
    string maritalStatus)
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;

    public DateTime BirthDate { get; set; } = birthDate;

    public string PhoneNumber { get; set; } = phoneNumber;

    public string MaritalStatus { get; set; } = maritalStatus;
    
    public override string ToString()
    {
        return $"{Name}, дата рождения: {BirthDate}, номер телефона: {PhoneNumber}, семейное положение: {MaritalStatus}";
    }
}