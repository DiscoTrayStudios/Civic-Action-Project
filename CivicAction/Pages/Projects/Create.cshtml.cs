using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


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

        var file = FileUpload.FormFile;

        if(file != null && file.Length > 0)
        {
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            string[] allowedExtension = {".jpg", "jpeg",".png",".gif"};

            if (!allowedExtension.Contains(extension))
            {
                ModelState.AddModelError("FileUpload.FormFile", "Only JPG, JPEG, PNG, and GIF files are allowed.");
                return Page();
            }

            if (file.Length > 2097152)
            {
                ModelState.AddModelError("FileUpload.FormFile", "Image must be smaller than 2 MB.");
                return Page();
            }
        }

        try
        {
            Inclusion Inclusion = new Inclusion
            {
                StudentId = user.Id,
                Student = user,
                // Check if the project is actually assigned here
                Project = Project,
                ProjectId = Project.Id,
                Hours = 0,
            };
            Project.IsWorkshop = false;
            context.Projects.Add(Project);
            context.Inclusions.Add(Inclusion);
            await context.SaveChangesAsync();

            /*File upload into folder*/ 
            /*https://learn.microsoft.com/en-us/answers/questions/807026/upload-image-to-asp-net-razor-page*/ 
            
            if (file != null && file.Length > 0)
            {   
                string folder = Path.Combine(hostEnvironment.WebRootPath, "uploadfiles");

                string fileName = $"project-{Project.Id}-{Guid.NewGuid():N}{Path.GetExtension(file.FileName).ToLowerInvariant()}";

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                } 

                Project.FilePath = "/uploadfiles/" + fileName;

                await context.SaveChangesAsync();
            } 
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error saving project: {ex.Message}");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}