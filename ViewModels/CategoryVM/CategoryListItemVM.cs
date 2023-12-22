namespace SitePustok.ViewModels.CategoryVM;

public class CategoryListItemVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsDeleted { get; set; }
}
