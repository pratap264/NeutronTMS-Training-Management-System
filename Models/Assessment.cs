using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeutronTMS.Models;

public class Assessment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public int? TrainingId { get; set; }

    [ForeignKey("TrainingId")]
    public Trainings? Training { get; set; }

    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public string DocumentLink { get; set; } = null!;
    [Required]
    public string SubmissionLink { get; set; } = null!;

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