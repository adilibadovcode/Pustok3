using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePustok.Contexts;
using SitePustok.Models;
using SitePustok.ViewModels.CategoryVM;
using SitePustok.ViewModels.TagVM;

namespace SitePustok.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Moderator")]

    public class CategoryController : Controller
	{
		PustokDBContext _db { get; }

		public CategoryController(PustokDBContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _db.Categories.Select(c => new CategoryListItemVM
			{
				Id = c.Id,
				Name = c.Name,
				ParentCategoryId = c.ParentCategoryId,
				IsDeleted = c.IsDeleted,
			}).Take(5).ToListAsync());


		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]

		public async Task<IActionResult> Create(CategoryCreateVM vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			if (await _db.Categories.AnyAsync(x => x.Name == vm.Name))
			{
				ModelState.AddModelError("Name", "This Name Already Exist");
				return View(vm);
			}
			Category category = new Category
			{
				Name = vm.Name,
				ParentCategoryId = vm.ParentCategoryId,
				IsDeleted = vm.IsDeleted switch
				{
					true => true,
					false => false
				},

			};
			await _db.Categories.AddAsync(category);
			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


        public async Task<IActionResult> Update(int? Id)
        {
            if (Id == null || Id <= 0) return BadRequest();
            var data = await _db.Categories.FindAsync(Id);
            if (data == null) return NotFound();
            return View(new CategoryUpdateVM
            {
                Id = data.Id,
                Name = data.Name,
				ParentCategoryId = data.ParentCategoryId,
				IsDeleted=data.IsDeleted
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? Id, CategoryUpdateVM vm)
        {
            if (Id == null || Id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Categories.FindAsync(Id);
            if (data == null) return NotFound();
			data.Name = vm.Name;
			data.ParentCategoryId = vm.ParentCategoryId;
			data.IsDeleted	= vm.IsDeleted;
	
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		public async Task<IActionResult> Delete(int? Id)
		{
			TempData["Respnose"] = false;
			if (Id == null) return BadRequest();
			var data = await _db.Categories.FindAsync(Id);
			if (data == null) return NotFound();
			_db.Categories.Remove(data);
			await _db.SaveChangesAsync();
			TempData["Respnose"] = true;
			return RedirectToAction(nameof(Index));
		}

		public IActionResult ShowMoreButton(int page = 1, int pageSize = 5)
		{
			var records = _db.Categories.ToList()
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

				return View(records);
		}
		public IActionResult GetMoreRecords(int page = 1, int pageSize = 5)
		{
			var records = _db.Categories.Select(c => new CategoryListItemVM
			{
				Id = c.Id,
				Name = c.Name,
				ParentCategoryId = c.ParentCategoryId,
				IsDeleted = c.IsDeleted,
			})
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

            return PartialView("_RecordPartial", records);

		}
	}
}
