using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			//if (!_context.Products.Any())
			//{
			//	CategoryModel yonexRacket = new CategoryModel{Name= "Yonex Racket", Slug="yonexracket", Description="Yonex is a large brand about badminton", Status=1};
			//	CategoryModel liningRacket = new CategoryModel{Name= "Lining Racket", Slug="liningracket", Description="Lining is a Chinese brand", Status=1};

			//	BrandModel yonex = new BrandModel { Name = "Yonex", Slug = "yonex", Description = "Yonex is a large brand about badminton", Status = 1 };
			//	BrandModel lining = new BrandModel { Name = "Lining", Slug = "lining", Description = "Lining is a Chinese brand", Status = 1 };
			//	_context.Products.AddRange(
			//		new ProductModel { Name="Astrox 77 Pro", Slug="astrox77pro", Description="Best racket ever", Image="astrox77pro.jpg", Category=yonexRacket,Brand=yonex, Price = 26},
			//		new ProductModel { Name="Lining Tectonic 6", Slug="liningtectonic6", Description="Most valueable wimme", Image= "liningtectonic6.webp", Category=liningRacket,Brand=lining, Price=13}
			//	);
			//}
			//else
			//{
			//Cập nhật dữ liệu image của sản phẩm Astrox 77 Pro
			//var product = _context.Products.FirstOrDefault(p => p.Name == "Astrox 77 Pro");
			//if (product != null)
			//{
			//	product.Image = "astrox77pro.webp";
			//	_context.Products.Update(product);
			//}

			//var product = _context.Products.FirstOrDefault(p => p.Slug == "65z3");
			//if (product != null)
			//{
			//	product.Image = "yonex65z3.webp";
			//	_context.Products.Update(product);
			//}
			//}
			//var brand = _context.Brands.FirstOrDefault(p => p.Name == "Yonex");
			//var category = _context.Categories.FirstOrDefault(p => p.Name == "Giày Yonex");
			//CategoryModel yonexShoes = new CategoryModel();
			//if(category == null)
			//{
			//	yonexShoes = new CategoryModel { Name = "Yonex Shoes", Slug = "yonexShoes", Description = "Every Shoes are good", Status = 1 };
			//}
			//if (brand != null)
			//{
			//	_context.Products.Add(
			//		new ProductModel { Name="Giày Yonex 65Z3", Slug = "65z3", Description="Best shoes ever", Image="65z3.webp", Category= yonexShoes, Brand = brand, Price = 15}
			//		);
			//}
			_context.SaveChanges();
		}
	}
}
