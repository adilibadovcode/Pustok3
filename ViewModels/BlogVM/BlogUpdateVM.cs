using SitePustok.Models;

namespace SitePustok.ViewModels.BlogVM
{
    public class BlogUpdateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AuthorId { get; set; }
        public IEnumerable<Author>? Author { get; set; }
        public List<int>? tagsId { get; set; }
        public IEnumerable<Tag>? tags { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<BlogTag> BlogTag { get; internal set; }
    }
}
