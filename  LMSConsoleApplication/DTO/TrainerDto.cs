using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.DTO;

public class TrainerDto
{
    public string Id { get; set; }
    public FullName FullName { get; set; }
    public Email Email { get; set; }
}