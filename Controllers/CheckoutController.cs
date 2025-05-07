using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Areas.Admin.Repository;
using WebApp.Models;
using WebApp.Repository;

namespace WebApp.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IEmailSender _emailSender;
		public CheckoutController(DataContext dataContext, IEmailSender emailSender)
		{
			_dataContext = dataContext;
			_emailSender = emailSender;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if(userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var orderCode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();

				var CouponCode = Request.Cookies["CouponTitle"];
				orderItem.CouponCode = CouponCode;

				orderItem.OrderCode = orderCode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;

				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();

				List<CartModel> cartItems = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();
				
				foreach(var cart in cartItems)
				{
					var orderDetails = new OrderDetail();
					orderDetails.UserName = userEmail;
					orderDetails.OrderCode = orderCode;
					orderDetails.ProductId = cart.ProductId;
					orderDetails.Price = cart.Price;
					orderDetails.Quantity = cart.Quantity;

					_dataContext.Add(orderDetails);
					_dataContext.SaveChanges();
				}
				TempData["success"] = "Checkout success!";
				HttpContext.Session.Remove("Cart");

				var receiver = "stu725105104@hnue.edu.vn";
				var subject = "Checkout success";
				var message = "Đặt hàng thành công, trải nghiệm dịch vụ nhé.";

				await _emailSender.SendEmailAsync(receiver, subject, message);
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}
	}
}
