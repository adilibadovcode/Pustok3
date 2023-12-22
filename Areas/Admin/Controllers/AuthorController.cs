using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePustok.Contexts;
using SitePustok.Models;
using SitePustok.ViewModels.AuthorVM;

namespace SitePustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="SuperAdmin,Admin,Moderator")]
    public class AuthorController : Controller
	{
		PustokDBContext _db { get; }

        public AuthorController(PustokDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
		{
            return View(await _db.Author.Select(c => new AuthorListItemVM
            {
                Id = c.Id,
                Name = c.Name,
                Surname = c.Surname,
                IsDeleted= c.IsDeleted,

            }).ToListAsync());
        }
        public async Task<IActionResult> Create(AuthorCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Author.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", "This Name Already Exist");
                return View(vm);
            }
            Author author = new Author
            {
                Name = vm.Name,
                Surname = vm.Surname,
                IsDeleted = vm.IsDeleted,
            };
            await _db.Author.AddAsync(author);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return BadRequest();

            var data = await _db.Author.FindAsync(Id);
            if (data == null) return NotFound();
            _db.Author.Remove(data);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? Id)
        {
            if (Id == null || Id <= 0) return BadRequest();
            var data = await _db.Author.FindAsync(Id);
            if (data == null) return NotFound();
            return View(new AuthorUpdateVM
            {
                Id = data.Id,
                Name = data.Name,
                Surname = data.Surname,
                IsDeleted = data.IsDeleted,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? Id, AuthorUpdateVM vm)
        {
            if (Id == null || Id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Author.FindAsync(Id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.Surname = vm.Surname;
            data.IsDeleted = vm.IsDeleted; 
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
