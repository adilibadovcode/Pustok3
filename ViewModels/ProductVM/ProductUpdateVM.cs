using SitePustok.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace SitePustok.ViewModels.ProductVM
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Availability { get; set; } = true;
        public int Qunatity { get; set; }
        public string Brand { get; set; }
        public int RewardPoint { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public IFormFile ImageFile { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal CostPrice { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public bool IsDeleted {  get; set; }
    }
}
