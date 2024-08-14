using btlAspnet.Models;
using btlAspnet.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace btlAspnet.Areas.Admin.Controllers
{
    [Route("Admin/Product")]
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ShoppingContext context;

        public ProductController(ShoppingContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var products = context.Products.Include("Category").ToList();

            return View(products);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            var categories = context.Categories.ToList();

            // Tạo ViewModel để truyền đến View
            var model = new ProductCreateViewModel
            {
                Product = new btlAspnet.Models.DataModels.Product(),
                Categories = categories
            };
            return View(model);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model, IFormFile uploadFile)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                // save the image file
                string fileName = Path.GetFileName(uploadFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }
                model.Product.Image = fileName;
            }
            model.Product.CreatedDate = DateTime.Now;
            context.Products.Add(model.Product);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Product");


        }

        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            var categories = await context.Categories.ToListAsync();

            var model = new ProductCreateViewModel
            {
                Product = product,
                Categories = categories
            };
            return View(model);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductCreateViewModel model, IFormFile uploadFile)
        {
            var product = await context.Products.FindAsync(id);
            //Update Product's Information
            product.ProductName = model.Product.ProductName;
            product.Quantity = model.Product.Quantity;
            product.Description = model.Product.Description;
            product.Category = model.Product.Category;
            product.Price = model.Product.Price;
            product.PriceOld = model.Product.PriceOld;
            product.Status = model.Product.Status;
            product.CategoryId = model.Product.CategoryId;

            if (uploadFile != null && uploadFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    var oldImagePath = Path.Combine("wwwroot/images", product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Lưu ảnh mới
                string fileName = Path.GetFileName(uploadFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }
                product.Image = fileName;
            }
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            string imagePath = Path.Combine("wwwroot/images", product.Image);
            System.IO.File.Delete(imagePath);
            context.Products.Remove(product);
            context.SaveChanges(true);
            return RedirectToAction("Index", "Product");
        }
    }
}
