using System.ComponentModel.DataAnnotations;

namespace SitePustok.ViewModels.CategoryVM;

public class CategoryCreateVM
{
    [Required]
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsDeleted { get; set; }
}
