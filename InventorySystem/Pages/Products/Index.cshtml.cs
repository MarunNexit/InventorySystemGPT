using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Pages.Products
{
    public class IndexModel(ProductService productService) : PageModel
    {
        private readonly ProductService _productService = productService;

        public List<Product> ProductList { get; set; } = [];

        [BindProperty]
        public Product Product { get; set; } = new();

        public bool ShowModal { get; set; }
        public bool IsEditing { get; set; }

        public async Task OnGetAsync()
        {
            ProductList = await _productService.GetAllProductsAsync();
        }

        public async Task<IActionResult> OnPostLoadAddAsync()
        {
            Product = new Product();
            ProductList = await _productService.GetAllProductsAsync();
            ShowModal = true;
            IsEditing = false;
            return Page();
        }

        public async Task<IActionResult> OnPostLoadEditAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null)
            {
                TempData["ErrorMessage"] = $"Product with ID {id} not found.";
                return RedirectToPage();
            }

            Product = product;
            ProductList = await _productService.GetAllProductsAsync();
            ShowModal = true;
            IsEditing = true;
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                ProductList = await _productService.GetAllProductsAsync();
                ShowModal = true;
                IsEditing = Product.Id != 0;
                return Page();
            }

            if (Product.Id == 0)
                await _productService.AddAsync(Product);
            else
                await _productService.UpdateAsync(Product);

            return RedirectToPage();
        }

        public IActionResult OnPostCancelAsync() => RedirectToPage();
    }
}
