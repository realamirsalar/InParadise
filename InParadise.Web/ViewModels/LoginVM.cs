using System.ComponentModel.DataAnnotations;

namespace InParadise.Web.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید.")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")] public bool RememberMe { get; set; }

        public string? RedirectUrl { get; set; }
    }
}