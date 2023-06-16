using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeutronTMS.Models;

public class Trainings
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public int? ProjectId { get; set; }
    
    [ForeignKey("ProjectId")]
    public Projects? Project {get; set; }

    public string Department { get; set; } = null!;

    [Required]
    public string Mode { get; set; } = null!;

    [Required]
    public string TrainerName { get; set; } = null!;
    public string Status { get; set; } = null!;

    [Required]
    public string Venue { get; set; } = null!;

    [Required]
    public DateTime ScheduledAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string? CreatedById { get; set; }
    
    [ForeignKey("CreatedById")]
    public ApplicationUser? CreatedBy {get; set; }

}