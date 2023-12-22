using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePustok.Contexts;
using SitePustok.ViewModels.SliderVM;

namespace Diana.ViewComponents;
public class SliderViewComponent : ViewComponent
{
    PustokDBContext _context { get; }

    public SliderViewComponent(PustokDBContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Sliders.Select(s => new SliderListItemVM
        {
            Id = s.Id,
            ImageUrl = s.ImageUrl,
            IsLeft = s.IsLeft,
            IsRightText = s.IsRightText,
            Title = s.Title,
            Text = s.Text,
        }).ToListAsync());
    }
}