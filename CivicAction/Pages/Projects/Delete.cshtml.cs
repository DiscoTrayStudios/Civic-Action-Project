using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CivicAction.Data;
using CivicAction.Models;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.Build.Experimental.ProjectCache;

namespace CivicAction.Pages.Projects
{
    public class DeleteModel : PageModel
    {
        private readonly CivicAction.Data.CivicActionContext _context;
        public DeleteModel(CivicAction.Data.CivicActionContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; } = default!;
        

        public List<Inclusion> Inclusions { get; set; } = new List<Inclusion>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FirstOrDefaultAsync(m => m.Id == id);

            if (project is not null)
            {
                Project = project;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {   
                Project = project;
                if (true)
                {
                    var fileName = Path.GetFileName(project.FilePath);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadfiles", fileName);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                _context.Projects.Remove(Project);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
