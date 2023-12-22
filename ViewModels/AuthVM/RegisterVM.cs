using System.ComponentModel.DataAnnotations;

namespace SitePustok.ViewModels.AuthVM;

public class RegisterVM
{
    [Required(ErrorMessage = "Enter Fullname"), MaxLength(40)]
    public string Fullname { get; set; }
    [Required(ErrorMessage = "Enter Email"), DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Enter Username"), MaxLength(24)]
    public string Username { get; set; }
    [Required(ErrorMessage = "Enter Password"), DataType(DataType.Password), Compare(nameof(ConfirmPassword)), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong Password Type")]
    public string Password { get; set; }
    [Required, DataType(DataType.Password), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$",ErrorMessage ="Wrong Password Type")]

    public string ConfirmPassword { get; set; }
}
