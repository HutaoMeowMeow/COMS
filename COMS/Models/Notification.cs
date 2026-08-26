using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class Notification
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public Guid? ObstructionAlertId { get; set; }

    [Required]
    [MaxLength(50)]
    public string NotificationType { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Data { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Unread";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAt { get; set; }

    public User? User { get; set; }
    public ObstructionAlert? ObstructionAlert { get; set; }
}
