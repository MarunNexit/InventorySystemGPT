using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Pages.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventorySystem.Tests
{
    public class IndexModelTests
    {
        private ApplicationDbContext _context;
        private IndexModel _pageModel;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted(); // Clean slate each time

            _pageModel = new IndexModel(_context);
        }

        [Test]
        public async Task OnGetAsync_ShouldLoadProducts()
        {
            // Arrange
            _context.Products.Add(new Product { Name = "Test Product", Price = 10, Quantity = 1 });
            await _context.SaveChangesAsync();

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            Assert.That(_pageModel.ProductList, Is.Not.Null);
            Assert.That(_pageModel.ProductList, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task OnPostLoadAddAsync_ShouldPrepareAddModal()
        {
            // Act
            var result = await _pageModel.OnPostLoadAddAsync();

            // Assert
            Assert.That(_pageModel.ShowModal, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(_pageModel.IsEditing, Is.False);
                Assert.That(result, Is.Not.Null);
            });
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.IActionResult>());
        }

        [Test]
        public async Task OnPostLoadEditAsync_WithValidId_ShouldLoadProductForEditing()
        {
            // Arrange
            var product = new Product { Name = "Edit Me", Price = 5, Quantity = 3 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _pageModel.OnPostLoadEditAsync(product.Id);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(_pageModel.ShowModal, Is.True);
                Assert.That(_pageModel.IsEditing, Is.True);
                Assert.That(_pageModel.Product.Id, Is.EqualTo(product.Id));
                Assert.That(result, Is.Not.Null);
            });
        }

        [Test]
        public async Task OnPostLoadEditAsync_WithInvalidId_ShouldReturnPageWithError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var tempDataProvider = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _pageModel.TempData = tempDataProvider;

            // Act
            var result = await _pageModel.OnPostLoadEditAsync(9999);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_pageModel.ShowModal, Is.False);
                Assert.That(_pageModel.ModelState.IsValid, Is.True); // Assuming no ModelState error is added manually
                Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
                Assert.That(_pageModel.TempData["ErrorMessage"], Is.EqualTo("Product with ID 9999 not found."));
            });
        }

        [Test]
        public async Task OnPostSaveAsync_AddNewProduct_ShouldAddAndRedirect()
        {
            // Arrange
            _pageModel.Product = new Product { Name = "New Product", Price = 15, Quantity = 10 };

            // Act
            var result = await _pageModel.OnPostSaveAsync();

            // Assert
            var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.Name == "New Product");
            Assert.Multiple(() =>
            {
                Assert.That(productInDb, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.RedirectToPageResult>());
            });
        }

        [Test]
        public async Task OnPostSaveAsync_EditExistingProduct_ShouldUpdateAndRedirect()
        {
            // Arrange
            var product = new Product { Name = "Old Product", Price = 20, Quantity = 5 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Detach the tracked entity
            _context.Entry(product).State = EntityState.Detached;

            // Modify product
            _pageModel.Product = new Product { Id = product.Id, Name = "Updated Product", Price = 25, Quantity = 7 };

            // Act
            var result = await _pageModel.OnPostSaveAsync();

            // Assert
            var productInDb = await _context.Products.FindAsync(product.Id);
            Assert.Multiple(() =>
            {
                Assert.That(productInDb.Name, Is.EqualTo("Updated Product"));
                Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.RedirectToPageResult>());
            });
        }

        [Test]
        public async Task OnPostSaveAsync_InvalidModel_ShouldReturnPageWithModal()
        {
            // Arrange
            _pageModel.Product = new Product(); // Missing required fields
            _pageModel.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _pageModel.OnPostSaveAsync();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(_pageModel.ShowModal, Is.True);
                Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.IActionResult>());
            });
        }

        [Test]
        public void OnPostCancelAsync_ShouldRedirectToPage()
        {
            // Act
            var result = _pageModel.OnPostCancelAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.RedirectToPageResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}