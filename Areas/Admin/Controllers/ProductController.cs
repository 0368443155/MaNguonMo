using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Repository;
using System.IO;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _environment;
		public ProductController(DataContext context, IWebHostEnvironment environment)
		{
			_dataContext = context;
			_environment = environment;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}

		public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

			if (ModelState.IsValid) //nếu các dữ liệu thỏa mãn
			{
				//them du lieu
				TempData["success"] = "Model is valid";
				product.Slug = product.Name.Replace(" ", "-");
				var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Existed Product!");
					return View(product);
				}
				if (product.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_environment.WebRootPath, "images"); //upload hình ảnh vào wwwroot
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName; // lấy tên file ảnh gắn với 1 đoạn mã
					string filePath = Path.Combine(uploadDir, imageName); //lấy đường dẫn

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					product.Image = imageName; //lấy đường dẫn ảnh đưa vào db
				}

				_dataContext.Add(product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Added product successfully";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Something wrong!";
				List<string> errorsList = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errorsList.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errorsList);
				return BadRequest(errorMessage);
			}

			return View(product);
		}
		public async Task<IActionResult> Edit(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");

			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int Id, ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

			var existed_product = _dataContext.Products.Find(product.Id);

			if (ModelState.IsValid) //nếu các dữ liệu thỏa mãn
			{
				if (product.ImageUpload != null)
				{
					//upload pic
					string uploadDir = Path.Combine(_environment.WebRootPath, "images"); //upload hình ảnh vào wwwroot
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName; // lấy tên file ảnh gắn với 1 đoạn mã
					string filePath = Path.Combine(uploadDir, imageName); //lấy đường dẫn

					//delete old pic
					string oldImage = Path.Combine(uploadDir, existed_product.Image); //lấy đường dẫn
					try
					{
						if (System.IO.File.Exists(oldImage))
						{
							System.IO.File.Delete(oldImage);
						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("", "An error occcured");
					}

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					existed_product.Image = imageName; //lấy đường dẫn ảnh đưa vào db
				}

				//Update properties
				existed_product.Name = product.Name;
				existed_product.Price = product.Price;
				existed_product.Description = product.Description;
				existed_product.CategoryId = product.CategoryId;
				existed_product.BrandId = product.BrandId;

				_dataContext.Update(existed_product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Updated product successfully";
				return RedirectToAction("Index");

			}
			else
			{
				TempData["error"] = "Something wrong!";
				List<string> errorsList = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errorsList.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errorsList);
				return BadRequest(errorMessage);
			}

			return View(product);
		}
		public async Task<IActionResult> Delete(int Id)
		{
			ProductModel product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == Id);
			if (!string.IsNullOrEmpty(product.Image))
			{
				string uploadDir = Path.Combine(_environment.WebRootPath, "images"); //upload hình ảnh vào wwwroot
				string oldImage = Path.Combine(uploadDir, product.Image); //lấy đường dẫn
				if (System.IO.File.Exists(oldImage))
				{
					System.IO.File.Delete(oldImage);
				}
			}
			_dataContext.Products.Remove(product);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Product deleted successfully";
			return RedirectToAction("Index");
		}
	}
}
