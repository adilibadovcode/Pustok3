using Microsoft.EntityFrameworkCore;
using SitePustok.Contexts;
using SitePustok.Models;

namespace SitePustok.Helpers;
public class LayoutService
{
    PustokDBContext _db { get; }

    public LayoutService(PustokDBContext db)
    {
        _db = db;
    }
    public async Task<Setting> GetSettingAsync() => await _db.Settings.FindAsync(1);
}
