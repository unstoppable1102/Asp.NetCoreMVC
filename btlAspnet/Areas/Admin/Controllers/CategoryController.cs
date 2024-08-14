
using Microsoft.AspNetCore.Mvc;
using btlAspnet.Models.BusinessModels;
using btlAspnet.Models.DataModels;
using Microsoft.AspNetCore.Authorization;


namespace btlAspnet.Areas.Admin.Controllers
{
    [Route("Admin/Category")]
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ShoppingContext context;
        public CategoryController(ShoppingContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(Category category, IFormFile uploadFile)
        {
            if (category.Image == null)
            {
                ModelState.AddModelError("Image", "The image field is required");
            }
            // save the image file
            string FileName = uploadFile.FileName;
            var filePath = Path.Combine("wwwroot/images", FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFile.CopyToAsync(stream);
            }

            category.Image = FileName;
            context.Categories.Add(category);
            context.SaveChanges();

            return RedirectToAction("Index", "Category");
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile uploadFile)
        {
            var existingCategory = context.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            if (uploadFile != null)
            {
                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingCategory.Image))
                {
                    var oldImagePath = Path.Combine("wwwroot/images", existingCategory.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                //Lưu ảnh mới
                string fileName = uploadFile.FileName;
                var filePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }
                existingCategory.Image = fileName;
            }
            else
            {
                existingCategory.Image = existingCategory.Image;
            }
            //update another attribute
            existingCategory.Name = category.Name;
            existingCategory.IsActive = category.IsActive;

            context.Categories.Update(existingCategory);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Category");
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = context.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("Index", "Category");
            }    
            string imagePath = Path.Combine("wwwroot/images", category.Image);
            System.IO.File.Delete(imagePath);
            context.Categories.Remove(category);
            context.SaveChanges(true);
            return RedirectToAction("Index", "Category");
        }



    }
}
