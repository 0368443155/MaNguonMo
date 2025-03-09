
namespace WebApp.Models.ViewModels
{
	public class CartViewModel
	{
		public List<CartModel> CartItems { get; set; }
		public decimal GrandTotal {  get; set; }
	}
}
