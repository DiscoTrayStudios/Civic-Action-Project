using System.ComponentModel.DataAnnotations;

namespace CivicAction.Models;

public enum ProjectType 
{
    Individual, Group, Workshop
}

public class Project
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; } = string.Empty;

    public double Hours { get; set; }
    public string? Organization { get; set; } = string.Empty;

    [Display(Name = "Site Location")]
    public string SiteLocation { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    
    [Display (Name = "Start Date")]
    public DateOnly Start { get; set; }

    [Display (Name = "End Date")]
    public DateOnly End { get; set; }

    [Display(Name = "Approved")]
    public bool IsApproved { get; set; }

    [Display(Name = "Workshop")]
    public bool IsWorkshop { get; set; }
    [Display(Name = "Archived")]
    public bool IsArchived { get; set; }

    public ICollection<Verification> Verifications { get; set; } = new List<Verification>();
}