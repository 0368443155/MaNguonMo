using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Repository;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController (DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{
			if (ModelState.IsValid) //nếu các dữ liệu thỏa mãn
			{
				//them du lieu
				TempData["success"] = "Model is valid";
				category.Slug = category.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Category Existed!");
					return View(category);
				}
				else
				{
					_dataContext.Add(category);
					await _dataContext.SaveChangesAsync();
					TempData["success"] = "Added category successfully";
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

			return View(category);
		}
		public async Task<IActionResult> Delete(int Id)
		{
			CategoryModel category = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Id == Id);
			
			_dataContext.Categories.Remove(category);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int Id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(Id);
			return View(category);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryModel category)
		{
			if (ModelState.IsValid) //nếu các dữ liệu thỏa mãn
			{
				//them du lieu
				category.Slug = category.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);

				if (slug != null)
				{
					ModelState.AddModelError("", "Category Existed!");
					return View(category);
				}

				_dataContext.Update(category);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Added category successfully";
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

			return View(category);
		}
	}
}
