using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CivicAction.Data;
using CivicAction.Models;

namespace CivicAction.Pages.Projects
{
    public class DetailsModel : PageModel
    {
        private readonly CivicAction.Data.CivicActionContext _context;

        public DetailsModel(CivicAction.Data.CivicActionContext context)
        {
            _context = context;
        }

        public Project Project { get; set; } = default!;

        public ICollection<Update> Updates { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                    .Include(p => p.Inclusions)
                    .ThenInclude(i => i.Student)
                    .FirstOrDefaultAsync(m => m.Id == id);
            
            var updates = _context.Updates
                    .Where(u => u.ProjectId == project.Id && u.Status == Status.Approved)
                    .ToList();

            if (project is not null)
            {
                Project = project;
                Updates = updates;

                return Page();
            }

            return NotFound();
        }
    }
}
