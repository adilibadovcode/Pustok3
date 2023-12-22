using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SitePustok.Areas.Admin.ViewModels;
using SitePustok.Contexts;
using SitePustok.ViewModels.BasketVM;
using SitePustok.ViewModels.CommonVM;
using SitePustok.ViewModels.HomeVM;

namespace SitePustok.Contollers
{
    public class HomeController : Controller
    {
        PustokDBContext _db { get; }

        public HomeController(PustokDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int take = 4;
            var items = _db.Products.Where(p => !p.IsDeleted).Take(take).Select(s => new AdminProductListItemVM
            {
                Title = s.Title,
                Availability = s.Availability,
                Category = s.Category,
                CategoryId = s.CategoryId,
                CostPrice = s.CostPrice,
                Discount = s.Discount,
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                IsDeleted = s.IsDeleted,
                Qunatity = s.Qunatity,
                SellPrice = s.SellPrice,
            });
            int count = await _db.Products.CountAsync(x => !x.IsDeleted);
            int last = (int)Math.Ceiling((decimal)count / 8);
            PaginationVM<IEnumerable<AdminProductListItemVM>> pag = new(count, 1, items, last);

            HomeVM vm = new HomeVM

            {
                Products = await _db.Products.Where(p => !p.IsDeleted).Select(s => new AdminProductListItemVM
                {
                    Title = s.Title,
                    Availability = s.Availability,
                    Category = s.Category,
                    CategoryId = s.CategoryId,
                    CostPrice = s.CostPrice,
                    Discount = s.Discount,
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    IsDeleted = s.IsDeleted,
                    Qunatity = s.Qunatity,
                    SellPrice = s.SellPrice,
                }).ToListAsync(),
                PaginatedProducts = pag
            };
            return View(vm);
        }
        public async Task<IActionResult> ProductPagination(int page = 1, int count = 2)
        {
            var items = _db.Products.Where(p => !p.IsDeleted).Skip((page - 1) * count).Take(count).Select(s => new AdminProductListItemVM
            {
                Title = s.Title,
                Availability = s.Availability,
                Category = s.Category,
                CategoryId = s.CategoryId,
                CostPrice = s.CostPrice,
                Discount = s.Discount,
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                IsDeleted = s.IsDeleted,
                Qunatity = s.Qunatity,
                SellPrice = s.SellPrice,
            });
            int totalCount = await _db.Products.CountAsync(x => !x.IsDeleted);
            int last = (int)Math.Ceiling((decimal)totalCount / count);
            PaginationVM<IEnumerable<AdminProductListItemVM>> pag = new(totalCount, page, items, last);
            return PartialView("_ProductPaginationPartial", pag);
        }

        //public string GetSession(string key)
        //{
        //    return HttpContext.Session.GetString(key) ?? "";
        //    //HttpContext.Session.Remove(key);
        //}
        //public void SetSession(string key, string value)
        //{
        //    HttpContext.Session.SetString(key, value);
        //}

        ///*Eslnde ProductControllerde yazmaliyam ama yaratmamisam*/

        public async Task<IActionResult> AddBasket(int? Id)
        {
            if (Id == null || Id < 0) return BadRequest();
            if (!await _db.Products.AnyAsync(p => p.Id == Id)) return NotFound();
            var basket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM?>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var existItem = basket.Find(b => b.Id == (int)Id);
            if (existItem == null)
            {
                basket.Add(new BasketProductAndCountVM
                {

                    Id = (int)Id,
                    Count = 1
                });
            }
            else
            {
                existItem.Count++;
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            return Ok();
        }
        public string GetCookie(string key)
        {
            return HttpContext.Request.Cookies[key] ?? "";
        }
        public IActionResult GetBasket()
        {
            return ViewComponent("Basket");
        }
        //public void SetCookie(string key, string value)
        //{
        //    HttpContext.Response.Cookies.Append(key, value, new CookieOptions
        //    {
        //           MaxAge=TimeSpan.FromMinutes(40)
        //    });
        //    //HttpContext.Response.Cookies.Delete(key);
        //}
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}