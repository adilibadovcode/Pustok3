using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePustok.Contexts;
using SitePustok.Models;
using SitePustok.ViewModels.TagVM;

namespace SitePustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Moderator")]

    public class TagController : Controller
	{
		PustokDBContext _db { get; }

		public TagController(PustokDBContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _db.Tag.Select(c => new TagListItemVM
			{
				Id = c.Id,
				Name = c.Name,

			}).ToListAsync());
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]

		public async Task<IActionResult> Create(TagCreateVM vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			if (await _db.Tag.AnyAsync(x => x.Name == vm.Name))
			{
				ModelState.AddModelError("Name", "This Name Already Exist");
				return View(vm);
			}
			Tag Tag = new Tag
			{
				Name = vm.Name,
			};
			await _db.Tag.AddAsync(Tag);
			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> Update(int? Id)
        {
            if (Id == null || Id <= 0) return BadRequest();
            var data = await _db.Tag.FindAsync(Id);
            if (data == null) return NotFound();
            return View(new TagUpdateVM
            {
                Id = data.Id,
				Name=data.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? Id, TagUpdateVM vm)
        {
            if (Id == null || Id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Tag.FindAsync(Id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return BadRequest();

            var data = await _db.Tag.FindAsync(Id);
            if (data == null) return NotFound();
            _db.Tag.Remove(data);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}



