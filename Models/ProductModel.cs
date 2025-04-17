using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Repository.Validation;

namespace WebApp.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Description { get; set; }
		[Required]
		[Range(0.01, double.MaxValue)]
		[Column(TypeName = "decimal(8,2)")]
		public decimal Price { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Choose a Brand")]
		public int BrandId { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Choose a Category")]
		public int CategoryId { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Image {  get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile ImageUpload { get; set; }
	}
}
