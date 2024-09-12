﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.tracker
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
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
        ViewData["StorageId"] = new SelectList(_context.Storages, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public InventoryTracker InventoryTracker { get; set; } = default!;
        public Storage Storage { get; set; } 

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var existingTracker = await _context.InventoryTracker.Where(x => x.ProductId == InventoryTracker.ProductId && x.StorageId == InventoryTracker.StorageId).FirstOrDefaultAsync();
            if(existingTracker == null)
            {
				_context.InventoryTracker.Add(InventoryTracker);
				await _context.SaveChangesAsync();
			}

            Storage = await _context.InventoryTracker
                .Select(x => x.Storage)
                .Where(x => x.Id == InventoryTracker.StorageId)
                .FirstOrDefaultAsync();

            int quantity = 0;

            if (existingTracker != null)
            {
                quantity = (int)existingTracker.Quantity + (int)InventoryTracker.Quantity;
                existingTracker.Quantity = quantity;
            }
            else
            {
                quantity = (int)InventoryTracker.Quantity;
				InventoryTracker.Quantity = quantity;

			}
			Storage.CurrentStock += quantity;
            Storage.Updated = DateTime.Now;
			await _context.SaveChangesAsync();


			return RedirectToPage("./Index");
        }
    }
}