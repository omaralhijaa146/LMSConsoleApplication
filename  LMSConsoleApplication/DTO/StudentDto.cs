using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Enums;

namespace LMSConsoleApplication.DTO;

public class StudentDto
{
    public string Id { get; set; }
    public FullName FullName { get; set; }
    public Email Email { get;  set; }
    public StudentStatus Status { get; set; }
}
