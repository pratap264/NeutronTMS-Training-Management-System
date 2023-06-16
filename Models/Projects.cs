using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeutronTMS.Models;

public class Projects
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Department { get; set; } = null!;

    [Required]
    public string Mode { get; set; } = null!;

    [Required]
    public string Status { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string? CreatedById { get; set; }

    [ForeignKey("CreatedById")]
    public ApplicationUser? CreatedBy { get; set; }
}