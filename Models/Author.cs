namespace SitePustok.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<Blog>? BlogId { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
