using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Repository;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	
	[Authorize]
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
		}
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BrandModel brand)
		{
			if (ModelState.IsValid) //nếu các dữ liệu thỏa mãn
			{
				//them du lieu
				TempData["success"] = "Model is valid";
				brand.Slug = brand.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(b => b.Slug == brand.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Brand Existed!");
					return View(brand);
				}
				else
				{
					_dataContext.Add(brand);
					await _dataContext.SaveChangesAsync();
					TempData["success"] = "Added brand successfully";
					return RedirectToAction("Index");
				}
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

			return View(brand);
		}
		public async Task<IActionResult> Delete(int Id)
		{
			BrandModel brand = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Id == Id);

			_dataContext.Brands.Remove(brand);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Brand deleted successfully";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int Id)
		{
			BrandModel brand = await _dataContext.Brands.FindAsync(Id);
			return View(brand);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(BrandModel brand)
		{
			if (ModelState.IsValid) //nếu các dữ liệu thỏa mãn
			{
				//them du lieu
				TempData["success"] = "Model is valid";
				brand.Slug = brand.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(b => b.Slug == brand.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Brand Existed!");
					return View(brand);
				}
				else
				{
					_dataContext.Update(brand);
					await _dataContext.SaveChangesAsync();
					TempData["success"] = "Added brand successfully";
					return RedirectToAction("Index");
				}
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

			return View(brand);
		}
	}
}
