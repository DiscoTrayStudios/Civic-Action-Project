using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CivicAction.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly CivicAction.Data.CivicActionContext _context;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(CivicAction.Data.CivicActionContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Project> Project { get;set; } = default!;
        public AppUser? CurrentUser {get; set;}

        public async Task OnGetAsync()
        {   
            CurrentUser = await _userManager.GetUserAsync(User);
            string? userId = _userManager.GetUserId(User);
            IQueryable<Project> projectsIQ = from p in _context.Projects select p;
            if (!CurrentUser.IsAdmin)
            {
                var inclusions = await _context.Inclusions
                    .Include(i => i.Project)
                    .Where(i => i.StudentId == userId)
                    .ToListAsync();
                projectsIQ = projectsIQ.Where(p => inclusions.Select(i => i.ProjectId).Contains(p.Id));
            }
            else
            {
                var inclusions = await _context.Inclusions
                    .Include(i => i.Project)
                    .ToListAsync();

                projectsIQ = projectsIQ
                        .Where(p => inclusions.Select(i => i.ProjectId).Contains(p.Id) && p.IsWorkshop == false)
                        .Include(p => p.Inclusions)
                        .ThenInclude(i => i.Student);
            }
            Project = await projectsIQ.ToListAsync();
        }

        public double GetHours(Project project)
        {
            return _context.Updates
                    .Where(u => u.ProjectId == project.Id && u.Status == Status.Approved)
                    .Sum(u => u.Hours);
        }

        public Status GetStatus(Project project)
        {
            var updates = _context.Updates.Where(u => u.ProjectId == project.Id);
            
            if (updates.Where(u => u.Status == Status.Rejected).Any())
            {
                return Status.Rejected;
            }

            if (updates.Where(u => u.Status == Status.Pending).Any())
            {
                return Status.Pending;
            }

            return Status.Approved;
        }

        public void SetStatus(Project project)
        {
            var updates = _context.Updates.Where(u => u.ProjectId == project.Id);
            
            // if one of the updates is rejected, set status to rejected and return
            if (updates.Where(u => u.Status == Status.Rejected).Any())
            {
                project.Status = Status.Approved;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
                return;
            }

            // if one of the updates is pending, set status to pending and return
            if (updates.Where(u => u.Status == Status.Pending).Any())
            {
                project.Status = Status.Pending;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
                return;
            }

            // otherwise, set status to approved and return
            project.Status = Status.Approved;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }
            return;
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(p => p.Id == id);
        }
    }
}
