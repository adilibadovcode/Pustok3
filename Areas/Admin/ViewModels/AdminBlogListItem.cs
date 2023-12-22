using SitePustok.Models;
namespace SitePustok.Areas.Admin.ViewModels
{
    public class AdminBlogListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AuthorId { get; set; }
        public IEnumerable<Author>? Author { get; set; }
        public IEnumerable<Tag>? Tag { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
