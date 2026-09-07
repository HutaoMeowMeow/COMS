using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class Announcement
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    public Guid PostedByUserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PostedByRole { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Barangay { get; set; }

    [MaxLength(100)]
    public string? Municipality { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public User? PostedByUser { get; set; }
}
