namespace SitePustok.Models
{
    public class Blog
    {
        internal IEnumerable<int> tagsId;

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AuthorId { get; set; }
        public IEnumerable<Author>? Author { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
		public IEnumerable<BlogTag> BlogTag { get; set; }


    }
}
