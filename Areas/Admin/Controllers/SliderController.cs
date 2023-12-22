using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePustok.Contexts;
using SitePustok.Models;
using SitePustok.ViewModels.SliderVM;

namespace SitePustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Moderator")]
    //[AllowAnonymous]
    public class SliderController : Controller
    {
        PustokDBContext _db { get; }

        public SliderController(PustokDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Sliders.Select(sItem => new SliderListItemVM
            {
                Id = sItem.Id,
                Text = sItem.Text,
                Title = sItem.Title,
                ImageUrl = sItem.ImageUrl,
                IsLeft = sItem.IsLeft,
                IsRightText = sItem.IsRightText,
            }).ToListAsync();
            return View(items);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Slider slider = new Slider
            {
                Text = vm.Text,
                Title = vm.Title,
                ImageUrl = vm.ImageUrl,
                IsLeft = vm.IsLeft switch
                {
                    0 => true,
                    1 => false,
                },
                IsRightText = vm.IsRightText switch
                {
                    0 => true,
                    1 => false,
                },
            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return BadRequest();

            var data = await _db.Sliders.FindAsync(Id);
            if (data == null) return NotFound();
            _db.Sliders.Remove(data);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? Id)
        {
            if (Id == null || Id <= 0) return BadRequest();
            var data = await _db.Sliders.FindAsync(Id);
            if (data == null) return NotFound();
            return View(new SliderUpdateVM
            {
                ImageUrl = data.ImageUrl,
                Text = data.Text,
                Title = data.Title,
                IsLeft = data.IsLeft switch
                {
                    true => 1,
                    false => 0,
                },
                IsRightText = data.IsRightText switch
                {
                    true => 1,
                    false => 0,
                },
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? Id, SliderUpdateVM vm)
        {
            if (Id == null || Id <= 0) return BadRequest();
            if (vm.IsLeft < 0 || vm.IsLeft > 1)
            {
                ModelState.AddModelError("IsLeft", "Wrong Input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Sliders.FindAsync(Id);
            if (data == null) return NotFound();
            data.Text = vm.Text;
            data.Title = vm.Title;
            data.ImageUrl = vm.ImageUrl;
            data.IsLeft = vm.IsLeft switch
            {
                0 => true,
                1 => false,
            };
            data.IsRightText = vm.IsRightText switch
            {
                0 => true,
                1 => false,
            };
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
