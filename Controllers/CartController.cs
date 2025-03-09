using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Repository;

namespace WebApp.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			//lay tu httpcontext cac gia tri de tra ve cho cartItem hoac tao 1 list moi neu khong co gia tri nao nhan duoc tu httpContext
			List<CartModel> cartItems = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();
			CartViewModel cartViewModel = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			
			return View(cartViewModel);
		}

		public IActionResult Checkout()
		{
			return View("~/Views/Checkout/Index.cshtml");
		}

		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();

			CartModel cartItems = cart.Where(x => x.ProductId == Id).FirstOrDefault();

			if (cartItems == null)
			{
				cart.Add(new CartModel(product));
			}
			else
			{
				cartItems.Quantity += 1;
			}

			HttpContext.Session.SetJson("Cart", cart);

			return Redirect(Request.Headers["Referer"].ToString());
		}
	}
}
