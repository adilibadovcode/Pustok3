using SitePustok.Areas.Admin.ViewModels;
using SitePustok.Models;

namespace SitePustok.ViewModels.ProductVM
{
    public class ProductListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Availability { get; set; } = true;
        public int Qunatity { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string ImageUrl { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }
        public float Discount { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<AdminProductListItemVM> PaginatedProducts { get; set; }
    }
}
