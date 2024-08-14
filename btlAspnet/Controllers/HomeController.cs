using btlAspnet.Models;
using btlAspnet.Models.BusinessModels;
using btlAspnet.Models.DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace btlAspnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShoppingContext _context;
        public HomeController(ILogger<HomeController> logger, ShoppingContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var categories = _context.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.Image,
                    IsActive = c.IsActive,
                    ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
                }).ToList();

            var viewModel = new ProductCategoryViewModel
            {
                Products = products,
                Categories = categories
            };

            var productNA = _context.Products.OrderByDescending(c =>c.CreatedDate).Take(4);
            ViewBag.ProductNA = productNA;

            return View(viewModel);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    FullName = model.FullName,
                    CreatedDate = DateTime.Now,
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Home");
            }
            return View(model);
        }
		[Route("Home/Login")]
        [HttpGet]
		public IActionResult HomeLogin()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("BkapLoginScheme");
            return Redirect("/");
        }

        [HttpPost]
        [Route("Home/Login")]
		public IActionResult HomeLogin(string username, string password)
		{
			var user = _context.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
			if (user != null)
			{
				var identity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.GivenName, user.FullName),
                    new Claim(ClaimTypes.Role, "user")
				}, "user_login");
				var principal = new ClaimsPrincipal(identity);
				var login = HttpContext.SignInAsync("BkapLoginScheme", principal);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewBag.error = "<div class='alert alert-danger'>Đăng nhập sai</div>";
			}
			return View();
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
