using System.ComponentModel.DataAnnotations;

namespace SitePustok.ViewModels.SliderVM;
public class SliderCreateVM
{
    [Required(ErrorMessage = "Insert Photo")]
    public string ImageUrl { get; set; }
    [Required, MinLength(3), MaxLength(100)]
    public string Title { get; set; }
    [Required, MinLength(3), MaxLength(100)]
    public string Text { get; set; }
    public byte IsLeft { get; set; }
    public byte IsRightText { get; set; }
}
