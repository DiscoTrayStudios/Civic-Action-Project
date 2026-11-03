using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CivicAction.Data;
using CivicAction.Models;

namespace CivicAction.Pages.Students
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

        public IList<AppUser> Student { get;set; } = default!;
        public AppUser? CurrentUser { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string? userId = _userManager.GetUserId(User);
            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser?.IsAdmin == true)
            {
                Student = await _context.AppUsers.ToListAsync();
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Logout", new { area = "Identity" });
            }
        }
    }
}
