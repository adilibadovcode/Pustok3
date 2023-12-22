using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SitePustok.Contexts;
using SitePustok.ViewModels.BasketVM;

namespace SitePustok.ViewComponents;
public class BasketViewComponent : ViewComponent
{
    PustokDBContext _db { get; }

    public BasketViewComponent(PustokDBContext db)
    {
        _db = db;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var items = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
        var products = _db.Products.Where(p => items.Select(i => i.Id).Contains(p.Id));
        List<BasketProductItemVM> basketItems = new();
        foreach (var item in products)
        {
            basketItems.Add(new BasketProductItemVM
            {
                Id= item.Id,
                Name=item.Title,
                Discount=item.Discount,
                ImageUrl=item.ImageUrl,
                Price = item.SellPrice,
                Count = items.FirstOrDefault(x=>x.Id==item.Id).Count,

            });
        }
        return View(basketItems);
    }
}
