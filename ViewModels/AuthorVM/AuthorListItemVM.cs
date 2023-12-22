using SitePustok.Models;

namespace SitePustok.ViewModels.AuthorVM
{
	public class AuthorListItemVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}
