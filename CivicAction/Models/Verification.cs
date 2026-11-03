using System.ComponentModel.DataAnnotations;

namespace CivicAction.Models;

public class Verification
{
    public int Id { get; set; }
    [Display(Name = "Is Approved")]
    public bool? IsApproved { get; set; }
    public string? Feedback { get; set; } = string.Empty;
    public string? AdminId { get; set; }
    public AppUser? Admin { get; set; }

    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public string? StudentId { get; set; } = string.Empty;
    public AppUser? Student { get; set; }
    
    public ICollection<Update> Updates { get; set; } = new List<Update>();
}