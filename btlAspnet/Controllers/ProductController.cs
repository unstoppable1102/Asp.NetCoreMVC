using btlAspnet.Models;
using btlAspnet.Models.BusinessModels;
using btlAspnet.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace ASP._Net_Final.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShoppingContext _context;

        public ProductController(ShoppingContext context)
        {
            this._context = context;
        }

        public IActionResult Index(string search, string sort, double? minPrice, double? maxPrice)
        {
        
            var products = _context.Products.AsQueryable();
            var categories = _context.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
                }).ToList();

            ViewBag.CurrentSort = sort;
            ViewBag.SortOptions = new List<SelectListItem>
            {
                new SelectListItem{ Value = "default", Text ="Default", Selected = sort == "default"},
                new SelectListItem{ Value = "name_asc", Text ="Name ASC", Selected = sort == "name_asc"},
                new SelectListItem{ Value = "name_desc", Text ="Name DESC", Selected = sort == "name_desc"},
                new SelectListItem{ Value = "price_asc", Text ="Price ASC", Selected = sort == "price_asc"},
                new SelectListItem{ Value = "price_desc", Text ="Price DESC", Selected = sort == "price_desc"}
            };

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.ProductName.Contains(search));
            }

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                products = products.Where(x => x.Price >= minPrice && x.Price <= maxPrice);
            }

            switch (sort)
            {
                case "name_asc":
                    products = products.OrderBy(x => x.ProductName);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(x => x.ProductName);
                    break;
                case "price_asc":
                    products = products.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(x => x.Price);
                    break;
                default:
                    break;
            }

            ViewBag.MinPrice = minPrice ?? 40;
            ViewBag.MaxPrice = maxPrice ?? 300;

            var viewModel = new ProductCategoryViewModel
            {
                Products = products.ToList(),
                Categories = categories
            };

            ViewBag.CurrentFilter = search;

            return View(viewModel);
        }


        [Route("Product/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            var product = _context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            {
                return View("Error");
            }

            var productInCategory = _context.Products.Where(p => p.CategoryId == product.CategoryId && p.Id != id).ToList();
            ViewBag.ProductInCategory = productInCategory;
            return View(product);
        }

        public IActionResult Category(int id)
        {
            var products = _context.Products.Where(p => p.CategoryId == id).ToList();
            var categories = _context.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
                }).ToList();

            var viewModel = new ProductCategoryViewModel
            {
                Products = products,
                Categories = categories
            };

            

            return View("Index", viewModel);
        }

        public IActionResult Paging(int page = 1)
        {
            int pageSize = 3;
            var products = _context.Products.ToPagedList(page, pageSize);
            return View(products);
        }


    }
}
