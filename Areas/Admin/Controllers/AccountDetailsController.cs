using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SitePustok.Contexts;
using SitePustok.Models;
using SitePustok.ViewModels.UserDetailsVM;


namespace SitePustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AccountDetailsController : Controller
    {
        PustokDBContext _db { get; }
        UserManager<AppUser> _um { get; }

        public AccountDetailsController(PustokDBContext db, UserManager<AppUser> um)
        {
            _db = db;
            _um = um;
        }

        public async Task<IActionResult> Index(string? Name)
        {
            var data = await _um.FindByNameAsync(Name);
            UserDetailsItemVM vm = new UserDetailsItemVM();
            vm.Username = data.UserName;
            vm.Fullname = data.Fullname;
            vm.Email = data.Email;


            return View(vm);
        }
        public async Task<IActionResult> Update(int? Id)
        {
            if (Id == null || Id <= 0) return BadRequest();
            var data = await _db.AppUsers.FindAsync(Id);
            if (data == null) return NotFound();
            return View(new UserDetailsItemVM
            {
                Fullname = data.Fullname,
                Username = data.UserName,
                Email = data.Email,

            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? Id, UserDetailsItemVM vm)
        {
            if (Id == null || Id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.AppUsers.FindAsync(Id);
            if (data == null) return NotFound();
            data.Fullname = vm.Fullname;
            data.UserName = vm.Username;
            data.Email = vm.Email;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //public async Task<IActionResult> Delete(int? Id)
        //{

        //}
    }
}
