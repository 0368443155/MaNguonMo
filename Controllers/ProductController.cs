using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.ViewModels;
using WebApp.Repository;

namespace WebApp.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;	
		public ProductController (DataContext dataContext)
		{
			_dataContext = dataContext; 
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _dataContext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();

			ViewBag.Keyword = searchTerm;

			return View(products);
		}
		public async Task<IActionResult> Details(int Id = 0) 
		{
			if(Id == 0) return RedirectToAction("Index");

			var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();

			return View(productsById);
		}
	}
}
