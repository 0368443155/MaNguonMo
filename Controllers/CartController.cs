using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			//Nhận Coupon code từ cookie
			var coupon_code = Request.Cookies["CouponTitle"];
			CartViewModel cartViewModel = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price),
				CouponCode = coupon_code
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

			TempData["success"] = "Add item to cart successfully!";

			return Redirect(Request.Headers["Referer"].ToString());
		} 
		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();

			CartModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll( p => p.ProductId == Id );
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart",cart);
			}
			TempData["success"] = "Decrease item quantity successfully!";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int Id)
		{
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart");

			CartModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["success"] = "Increase item quantity successfully!";

			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Remove(int Id)
		{
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart");

			cart.RemoveAll(p => p.ProductId == Id);

			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");

			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Remove item successfully!";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Clear(int Id)
		{
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Clear cart successfully!";
			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Cart/GetCoupon")]
		public async Task<IActionResult> GetCoupon(CouponModel couponModel, string coupon_value)
		{
			var validCoupon = await _dataContext.Coupons
				.FirstOrDefaultAsync(x => x.Name == coupon_value && x.Quantity >= 1);

			string couponTitle = validCoupon.Name + " | " + validCoupon?.Description;

			if (couponTitle != null)
			{
				TimeSpan remainingTime = validCoupon.DateExpired - DateTime.Now;
				int daysRemaining = remainingTime.Days;

				if (daysRemaining >= 0)
				{
					try
					{
						var cookieOptions = new CookieOptions
						{
							HttpOnly = true,
							Expires = DateTimeOffset.UtcNow.AddMinutes(30),
							Secure = true,
							SameSite = SameSiteMode.Strict // Kiểm tra tính tương thích trình duyệt
						};

						Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
						return Ok(new { success = true, message = "Coupon applied successfully" });
					}
					catch (Exception ex)
					{
						//trả về lỗi 
						Console.WriteLine($"Error adding apply coupon cookie: {ex.Message}");
						return Ok(new { success = false, message = "Coupon applied failed" });
					}
				}
				else
				{

					return Ok(new { success = false, message = "Coupon has expired" });
				}

			}
			else
			{
				return Ok(new { success = false, message = "Coupon not existed" });
			}

			return Json(new { CouponTitle = couponTitle });
		}

	}
}
