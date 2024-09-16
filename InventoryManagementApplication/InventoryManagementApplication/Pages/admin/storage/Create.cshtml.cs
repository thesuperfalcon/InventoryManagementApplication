using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.storage
{
    public class CreateModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public CreateModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public Storage Storage { get; set; } = default!;
        public string StatusMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var storages = await _context.Storages.ToListAsync();
            if (storages.Count < 6)
            {
                _context.Storages.Add(Storage);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else
            {
                StatusMessage = "Max antal lagerplatser är 6";
                return Page();
            }
        }
    }
}
