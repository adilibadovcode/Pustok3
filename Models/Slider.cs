using System.ComponentModel.DataAnnotations;

namespace SitePustok.Models;

public class Slider
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Insert Photo")]
    public string ImageUrl { get; set; }
    [Required, MinLength(3), MaxLength(100)]
    public string Title { get; set; }
    [Required, MinLength(3), MaxLength(100)]
    public string Text { get; set; }
    public bool IsLeft { get; set; }
    public bool IsRightText { get; set; }
}
