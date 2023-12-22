namespace SitePustok.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<Product>? Products { get; set; }
}
