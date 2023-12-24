using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePustok.Areas.Admin.ViewModels;
using SitePustok.Contexts;
using SitePustok.Models;

namespace SitePustok.Contollers
{
    public class ProductController : Controller
    {
        PustokDBContext _db { get; }

        public ProductController(PustokDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? q, List<int>? authorids, List<int>? catids)
        {
            ViewBag.Categories = _db.Categories.Include(c => c.Products);
            ViewBag.Author = _db.Author;
            var query = _db.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.Title.Contains(q));
            }
            if (catids != null && catids.Any())
            {
                query = query.Where(p => catids.Contains(p.CategoryId));
            }
      
            return View(query.Select(s => new AdminProductListItemVM
            {
                Title = s.Title,
                Availability = s.Availability,
                CategoryId = s.CategoryId,
                CostPrice = s.CostPrice,
                Discount = s.Discount,
                ImageUrl = s.ImageUrl,
                SellPrice = s.SellPrice,
                Description=s.Description,

            }));
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(string? q, List<int>? authorids, List<int>? catids)
        //{
        //    ViewBag.Categories = _db.Categories.Include(c => c.Products);
        //    ViewBag.Author = _db.Author;
        //    return View();

        //}
    }
}
