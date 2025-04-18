using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class CouponModel
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		public DateTime DateStart { get; set; }
		public DateTime DateExpired { get; set; }
		[Required]
		public int Quantity { get; set; }
		public int Status { get; set; }
	}
}
