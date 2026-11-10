using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CivicAction.Pages.Projects;

[Authorize]
public class CreateModel(CivicActionContext context, UserManager<AppUser> userManager, IWebHostEnvironment hostEnvironment) : PageModel
{   
    [BindProperty]
    public Project Project { get; set; } = default!;

    [BindProperty]
    public FileViewModel FileUpload { get; set; } = new();


    public class FileViewModel 
    { 
    public IFormFile? FormFile { get; set; } 
    } 

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage("/Account/Login");

        // Remove StudentID from validation since we set it server-side
        //ModelState.Remove("Project.StudentID");
        
        if (!ModelState.IsValid) return Page();

        try
        {
            Project.StudentId = user.Id;
            Project.IsWorkshop = false;
            context.Projects.Add(Project);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error saving project: {ex.Message}");
            return Page();
        }

        /*File upload into folder*/ 
        /*https://learn.microsoft.com/en-us/answers/questions/807026/upload-image-to-asp-net-razor-page*/ 
        if (FileUpload.FormFile != null && FileUpload.FormFile.Length > 0)
        {   
            using (var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "uploadfiles", FileUpload.FormFile.FileName), FileMode.Create)) 
            { 
                await FileUpload.FormFile.CopyToAsync(stream); 
            } 
        } 
 

        return RedirectToPage("./Index");
    }
}