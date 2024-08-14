using btlAspnet.Models.BusinessModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace btlAspnet.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin")]
	[Authorize]
	public class AdminController : Controller
	{
		private readonly ShoppingContext context;
		public AdminController(ShoppingContext context)
		{
			this.context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		[Route("ProjectDashboard")]
		public IActionResult ProjectDashboard()
		{
			return View();
		}

		[Route("Login")]
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login(string RequestPath = null)
		{
            ViewData["RequestPath"] = RequestPath;
            return View();
        }

		[Route("Login")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string username, string password)
		{
			var user = context.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
			if (user != null)
			{
				if (user.IsAdmin) 
				{
					var identity = new ClaimsIdentity(new[]
					{
						new Claim(ClaimTypes.Name, user.UserName),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.GivenName, user.FullName),
						new Claim(ClaimTypes.Role, "Admin")
					}, "admin_login");

					var principal = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync("BkapLoginScheme", principal);
					return Redirect("~/admin");
				}
				else
				{
					ViewBag.error = "<div class='alert alert-danger'>Bạn không có quyền truy cập</div>";
				}	

			}
			else
			{
				ViewBag.error = "<div class='alert alert-danger'>Đăng nhập sai</div>";
			}
			return View();
		}

		[Route("Logout")]
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Logout()
		{
            HttpContext.SignOutAsync("BkapAdminLoginScheme");
            return Redirect("~/admin/login");
		}

		[Route("ProjectList")]
		public IActionResult ProjectList()
		{
			return View();
		}

		[Route("ScrumBoard")]
		public IActionResult ScrumBoard()
		{
			return View();
		}

		[Route("Issue")]
		public IActionResult Issue()
		{
			return View();
		}

		//CRM
		[Route("CrmDashboard")]
		public IActionResult CrmDashboard()
		{
			return View();
		}

		[Route("Calendar")]
		public IActionResult Calendar()
		{
			return View();
		}

		[Route("Customers")]
		public IActionResult Customers()
		{
			return View();
		}

		[Route("CustomerDetails")]
		public IActionResult CustomerDetails()
		{
			return View();
		}

        [Route("Mail")]
        public IActionResult Mail()
        {
            return View();
        }

		//sales
		[Route("SalesDashboard")]
		public IActionResult SalesDashboard()
		{
			return View();
		}
	}
}
