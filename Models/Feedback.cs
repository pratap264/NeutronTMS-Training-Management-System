using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeutronTMS.Models;

public class Feedback
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public int? TrainingId { get; set; }

    [ForeignKey("TrainingId")]
    public Trainings? Training { get; set; }

    public string FeedbackLink { get; set; } = null!;


    [Required]
    public string Status { get; set; } = null!;

    [Required]
    public DateTime EndDate { get; set; }

    public string? CreatedById { get; set; }

    [ForeignKey("CreatedById")]
    public ApplicationUser? CreatedBy { get; set; }
}