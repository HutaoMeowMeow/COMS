namespace COMS.DTOs;

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; }
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public bool? IsActive { get; set; }
}
