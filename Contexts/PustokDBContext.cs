using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SitePustok.Models;

namespace SitePustok.Contexts;


public class PustokDBContext : IdentityDbContext
{
    public PustokDBContext(DbContextOptions opt) : base(opt) { }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Blog> Blog { get; set; }
    public DbSet<Author> Author { get; set; }
    public DbSet<BlogTag> BlogTag { get; set; }
    public DbSet<ProductImage> ProductImage { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>()
            .HasData(new Setting
            {
                Address="Sumqayıt , Ceyranbatan",
                Email="Adilibadov456@gmail.com",
                Number="+994513317139",
                Logo= "~/image/logo.png",
                Id=1
            });
        base.OnModelCreating(modelBuilder);
    }

}
