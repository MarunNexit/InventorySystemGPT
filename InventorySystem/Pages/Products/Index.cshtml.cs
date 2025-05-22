using InventorySystem.Data;
using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Pages.Products
{
    public class IndexModel(ApplicationDbContext context) : PageModel
    {
        private readonly ApplicationDbContext _context = context;

        public List<Product> ProductList { get; set; } = [];

        [BindProperty]
        public Product Product { get; set; } = new();

        // Controls whether the modal is visible
        public bool ShowModal { get; set; } = false;

        // True if editing existing product, false if adding new
        public bool IsEditing { get; set; } = false;

        // Load product list on GET request
        public async Task OnGetAsync()
        {
            ProductList = await _context.Products.AsNoTracking().ToListAsync();
        }

        // Show Add Product modal
        public async Task<IActionResult> OnPostLoadAddAsync()
        {
            Product = new Product();
            ProductList = await _context.Products.AsNoTracking().ToListAsync();
            ShowModal = true;
            IsEditing = false;
            return Page();
        }

        // Show Edit Product modal
        public async Task<IActionResult> OnPostLoadEditAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                // Better user experience: redirect to page with error message
                TempData["ErrorMessage"] = $"Product with ID {id} not found.";
                return RedirectToPage();
            }

            Product = product;
            ProductList = await _context.Products.AsNoTracking().ToListAsync();
            ShowModal = true;
            IsEditing = true;
            return Page();
        }

        // Save new or updated product
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                ProductList = await _context.Products.AsNoTracking().ToListAsync();
                ShowModal = true;
                IsEditing = Product.Id != 0;
                return Page();
            }

            if (Product.Id == 0)
            {
                _context.Products.Add(Product);
            }
            else
            {
                _context.Attach(Product).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        // Cancel modal action - reload page without modal
        public IActionResult OnPostCancelAsync()
        {
            return RedirectToPage();
        }
    }
}
