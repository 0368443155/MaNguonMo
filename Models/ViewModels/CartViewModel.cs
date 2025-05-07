
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.ViewModels
{
	public class CartViewModel
	{
		public List<CartModel> CartItems { get; set; }
		public decimal GrandTotal {  get; set; }
		public string CouponCode { get; set; }
		public int DiscountPercent { get; set; }
	}
}
