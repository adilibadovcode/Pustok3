using Microsoft.AspNetCore.Mvc;
using SitePustok.Models;
using SitePustok.ViewModels.BlogVM;
using SitePustok.Contexts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace SitePustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Moderator")]

    public class BlogController : Controller
    {
        PustokDBContext _db { get; }

        public BlogController(PustokDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Blog.Select(p => new BlogListItemVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                AuthorId = p.AuthorId,
                IsDeleted = p.IsDeleted,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Tag = p.BlogTag.Select(bt => bt.Tag)
            }));
        }
        public IActionResult Create()
        {
            ViewBag.Tag = _db.Tag;
            ViewBag.Blog = _db.Blog;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tag = _db.Tag;
                ViewBag.Blog = _db.Blog;
                return View(vm);
            }
            Blog blog = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                AuthorId = vm.AuthorId,
                IsDeleted = vm.IsDeleted,
                BlogTag = vm.tagsId.Select(Id => new BlogTag
                {
                    TagId = Id

                }).ToList(),
            };
            await _db.Blog.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? Id)
        {
            TempData["Respnose"] = false;
            if (Id == null) return BadRequest();
            var data = await _db.Blog.FindAsync(Id);
            if (data == null) return NotFound();
            _db.Blog.Remove(data);
            await _db.SaveChangesAsync();
            TempData["Respnose"] = true;
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? Id)
        {
            if (Id == null || Id <= 0) return BadRequest();
            ViewBag.Tag = _db.Tag;
            var data = await _db.Blog
                .Include(p => p.BlogTag)
                .SingleOrDefaultAsync(p => p.Id == Id);
            var datatwo = await _db.Blog.FindAsync(Id);
            if (datatwo == null) return NotFound();
          
            return View(new BlogUpdateVM
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                AuthorId = data.AuthorId,
                IsDeleted = data.IsDeleted,
            });

    
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? Id, BlogUpdateVM vm)
        {
            if (Id == null || Id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Blog.FindAsync(Id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.AuthorId = vm.AuthorId;
            data.IsDeleted = vm.IsDeleted;
            data.Author = vm.Author;
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }

    }
}

