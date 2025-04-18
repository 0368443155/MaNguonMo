using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required]
		public string UserName { get; set; }
		[DataType(DataType.Password), Required]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
