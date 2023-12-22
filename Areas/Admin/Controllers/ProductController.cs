using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePustok.Areas.Admin.ViewModels;
using SitePustok.Contexts;
using SitePustok.Models;
using SitePustok.ViewModels.ProductVM;
using SitePustok.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using NuGet.Packaging;
using Microsoft.AspNetCore.Authorization;

namespace SitePustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Moderator")]

    public class ProductController : Controller
    {
        PustokDBContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductController(PustokDBContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {

            return View(_db.Products.Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                SellPrice = p.SellPrice,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                Category = p.Category,
                CategoryId = p.CategoryId,
                IsDeleted = p.IsDeleted,
                ImageUrl = p.ImageUrl,


            }));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImageFile.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }

            }

            if (vm.Images != null)
            {
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                    }
                    if (!img.IsValidSize(200))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");
                    }
                }

            }




            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.Products = _db.Products;
                return View(vm);
            }
            if (!await _db.Categories.AnyAsync(c => c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesnt exist");
                ViewBag.Products = _db.Products;
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }

            Product product = new Product
            {
                Title = vm.Title,
                Description = vm.Description,
                SellPrice = vm.SellPrice,
                CostPrice = vm.CostPrice,
                Availability = vm.Availability,
                Brand = vm.Brand,
                CategoryId = vm.CategoryId,
                Discount = vm.Discount,
                ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product),
                Qunatity = vm.Qunatity,
                RewardPoint = vm.RewardPoint,
                ProductImage = vm.Images.Select(i => new ProductImage
                {
                    ImageUrl = i.SaveAsync(PathConstants.Product).Result
                }).ToList(),
            };
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return BadRequest();

            var data = await _db.Products.FindAsync(Id);
            if (data == null) return NotFound();
            _db.Products.Remove(data);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
            //ViewBag.Categories = _db.Categories;
            var data = await _db.Products
                .Include(p => p.ProductImage)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (data == null) return NotFound();

            var vm = new ProductUpdateVM
            {
                CategoryId = data.CategoryId,
                CostPrice = data.CostPrice,
                Description = data.Description,
                Discount = data.Discount,
                SellPrice = data.SellPrice,

            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImageFile.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }
            }
            if (vm.Images != null)
            {
                //string message = string.Empty;
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                        //message += "Wrong file type (" + img.FileName + ") \r\n";
                    }
                    if (!img.IsValidSize(200))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");
                        //message += "Files length must be less than kb (" + img.FileName + ") \r\n";
                    }
                }
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(vm);
            }

            var data = await _db.Products
                .Include(p => p.ProductImage)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();

            if (vm.ImageFile != null)
            {
                string filePath = Path.Combine(PathConstants.RootPath, data.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                data.ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product);
            }
            if (vm.Images != null)
            {
                var imgs = vm.Images.Select(i => new ProductImage
                {
                    ImageUrl = i.SaveAsync(PathConstants.Product).Result,
                    ProductId = data.Id
                });

                data.ProductImage.AddRange(imgs);
            }


            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteImageCSharp(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.ProductImage.FindAsync(id);
            if (data == null) return NotFound();
            _db.ProductImage.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Update), new { id = data.ProductId });
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.ProductImage.FindAsync(id);
            if (data == null) return NotFound();
            _db.ProductImage.Remove(data);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}